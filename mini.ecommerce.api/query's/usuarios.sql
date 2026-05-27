CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE IF NOT EXISTS usuarios(
    id                  SERIAL                  PRIMARY KEY,
    nome                VARCHAR(100)            NOT NULL,
    sobrenome           VARCHAR(100)            NOT NULL,
    senha_hash          TEXT                    NOT NULL,
    tipo_usuario        VARCHAR(20)             NOT NULL,
    cnpj_cpf            VARCHAR(14)             NOT NULL UNIQUE,
    data_nascimento     DATE,
    foto_perfil_url     TEXT,
    ativo               BOOLEAN                 DEFAULT TRUE,
    ultimo_login        TIMESTAMP,
    dt_hr_criacao       TIMESTAMP               DEFAULT NOW(),
    dt_hr_atualizacao   TIMESTAMP               DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS usuario_email (
    id                  SERIAL                  PRIMARY KEY,
    usuario_id          INT                     NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    email               VARCHAR(150)            NOT NULL,
    principal           BOOLEAN                 DEFAULT FALSE,
    dt_criacao          TIMESTAMP               DEFAULT NOW(),
    CONSTRAINT uq_usuario_email UNIQUE (email)
);

CREATE TABLE IF NOT EXISTS usuario_telefone (
    id                  SERIAL                  PRIMARY KEY,
    usuario_id          INT                     NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    telefone            VARCHAR(30)             NOT NULL,
    principal           BOOLEAN                 DEFAULT FALSE,
    dt_criacao          TIMESTAMP               DEFAULT NOW(),
    CONSTRAINT uq_usuario_telefone UNIQUE (telefone)
);

CREATE TABLE IF NOT EXISTS log_eventos (
    id                  SERIAL                  PRIMARY KEY,
    usuario_id          INT                     NULL,
    acao                VARCHAR(50),
    status              VARCHAR(20),            -- SUCCESS | ERROR
    mensagem            TEXT,
    payload             JSONB,
    dt_hr               TIMESTAMP               DEFAULT NOW()
);

CREATE OR REPLACE FUNCTION fn_usuario_criar_completo(
    p_nome TEXT,
    p_sobrenome TEXT,
    p_email TEXT,
    p_senha TEXT,
    p_tipo_usuario TEXT,
    p_cnpj_cpf TEXT,
    p_data_nascimento DATE,
    p_foto_perfil_url TEXT DEFAULT NULL,
    p_telefone TEXT DEFAULT NULL
)
RETURNS JSON
LANGUAGE plpgsql
AS $$
DECLARE
    v_usuario_id INT;
    v_hash TEXT;
    v_email_sanitizado TEXT;
    v_cpf_cnpj_sanitizado TEXT;
    v_telefone_sanitizado TEXT;
    v_mensagem_erro TEXT;
BEGIN
    -- 1. SANITIZAÇÃO DE DADOS
    v_email_sanitizado := LOWER(TRIM(p_email)); 
    v_cpf_cnpj_sanitizado := regexp_replace(p_cnpj_cpf, '\D', '', 'g');
    
    IF p_telefone IS NOT NULL THEN
        v_telefone_sanitizado := regexp_replace(p_telefone, '\D', '', 'g');
    END IF;

    -- 2. VALIDAÇÕES DE NEGÓCIO (Fail-Fast)
    -- 2.1. Valida formato de E-mail via Regex
    IF v_email_sanitizado !~ '^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$' THEN
        RETURN json_build_object(
            'Status', 2, -- EnumStatus.NEGOCIO
            'ErrorObject', json_build_object(
                'tipoErro', 2,
                'codErro', 400, -- Bad Request
                'msgErro', 'Formato de e-mail inválido.',
                'origemErro', 'PostgreSQL (fn_usuario_criar_completo)'
            )
        );
    END IF;

    -- 2.2. Valida tamanho do CPF (11) ou CNPJ (14)
    IF length(v_cpf_cnpj_sanitizado) NOT IN (11, 14) THEN
        RETURN json_build_object(
            'Status', 2, -- EnumStatus.NEGOCIO
            'ErrorObject', json_build_object(
                'tipoErro', 2,
                'codErro', 400,
                'msgErro', 'CPF ou CNPJ inválido.',
                'origemErro', 'PostgreSQL (fn_usuario_criar_completo)'
            )
        );
    END IF;

    -- 3. GERAÇÃO DO HASH
    v_hash := crypt(p_senha, gen_salt('bf'));

    -- 4. INSERÇÕES
    INSERT INTO usuarios (
        nome, sobrenome, senha_hash, tipo_usuario, 
        cnpj_cpf, data_nascimento, foto_perfil_url
    )
    VALUES (
        p_nome, p_sobrenome, v_hash, p_tipo_usuario, 
        v_cpf_cnpj_sanitizado, p_data_nascimento, p_foto_perfil_url
    )
    RETURNING id INTO v_usuario_id;

    INSERT INTO usuario_email(usuario_id, email, principal)
    VALUES (v_usuario_id, v_email_sanitizado, TRUE);

    IF v_telefone_sanitizado IS NOT NULL AND v_telefone_sanitizado <> '' THEN
        INSERT INTO usuario_telefone(usuario_id, telefone, principal)
        VALUES (v_usuario_id, v_telefone_sanitizado, TRUE);
    END IF;

    -- 5. LOG DE SUCESSO 
    INSERT INTO log_eventos(usuario_id, acao, status, mensagem, payload)
    VALUES (v_usuario_id, 'CREATE_USER', 'SUCCESS', 'Usuário criado', jsonb_build_object('email', v_email_sanitizado));

    -- 6. RETORNO DE SUCESSO
    RETURN json_build_object(
        'Status', 1, -- EnumStatus.SUCESSO
        'SuccessObject', json_build_object(
            'id', v_usuario_id,
            'nome', p_nome,
            'email', v_email_sanitizado
        )
    );

EXCEPTION 
    -- 7. TRATAMENTO DE ERROS DE NEGÓCIO (Conflitos de Unique Key)
    WHEN unique_violation THEN 
        IF SQLERRM LIKE '%uq_usuario_email%' THEN
            v_mensagem_erro := 'O e-mail informado já está em uso.';
        ELSIF SQLERRM LIKE '%uq_usuario_telefone%' THEN
            v_mensagem_erro := 'O telefone informado já está em uso.';
        ELSIF SQLERRM LIKE '%usuarios_cnpj_cpf_key%' THEN
            v_mensagem_erro := 'O CPF/CNPJ informado já está cadastrado.';
        ELSE
            v_mensagem_erro := 'Conflito de dados já existentes no sistema.';
        END IF;

        -- Grava o erro na tabela de log internamente
        INSERT INTO log_eventos(acao, status, mensagem, payload)
        VALUES ('CREATE_USER', 'ERROR', v_mensagem_erro, jsonb_build_object('email', v_email_sanitizado));

        RETURN json_build_object(
            'Status', 2, -- EnumStatus.NEGOCIO
            'ErrorObject', json_build_object(
                'tipoErro', 2,
                'codErro', 409, -- Conflict
                'msgErro', v_mensagem_erro,
                'origemErro', 'PostgreSQL (fn_usuario_criar_completo)'
            )
        );

    -- 8. TRATAMENTO DE ERROS DE SISTEMA (Exceções graves)
    WHEN OTHERS THEN
        INSERT INTO log_eventos(acao, status, mensagem, payload)
        VALUES ('CREATE_USER', 'ERROR', 'Falha interna ao criar usuário.', jsonb_build_object('erro_sql', SQLERRM, 'estado', SQLSTATE));

        RETURN json_build_object(
            'Status', 3, -- EnumStatus.SISTEMA
            'ErrorObject', json_build_object(
                'tipoErro', 3,
                'codErro', 500, -- Internal Server Error
                'msgErro', 'Ocorreu um erro interno no servidor de banco de dados.',
                'origemErro', 'PostgreSQL (fn_usuario_criar_completo)'
            )
        );
END;
$$;