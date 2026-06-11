# Processo Seletivo LAPES 2026

## 👤 Candidatos

Ramon Souza

Yslan Lopes

## 🎯 Trilha(s)

Trilha de Desenvolvimento (Mini E-commerce)

## 📞 Contato

Email: 

[ramon14souza@gmail.com](mailto:ramon14souza@gmail.com)

[yslan.contato@gmail.com](mailto:yslan.contato@gmail.com)

WhatsApp: 

(91) 9 9365-0461 -> Ramon

(91) 9 8966-5188 -> Yslan

## 🚀 Como rodar o projeto

```bash
(TEM QUE BOTAR O TUTO)
```

## 🛠️ Tecnologias utilizadas

* DOTNET 8
* React
* PostgreSQL
* Docker

## 🏗️ Arquiteturas utilizadas

* Hexagonal
* DDD (Domain Driven Design) 

## 🧠 Decisões técnicas

A escolha das tecnologias foi guiada principalmente por experiência prática e familiaridade, buscando garantir produtividade, qualidade de código e facilidade na manutenção do sistema.

* .NET 8 foi utilizado no backend por ser a tecnologia que utilizo diariamente no ambiente de trabalho, o que permite maior domínio sobre boas práticas, organização do código e implementação de padrões como DDD e Arquitetura Hexagonal.

* PostgreSQL foi adotado como banco de dados devido à sua estabilidade, alta performance, conformidade com padrões SQL e suporte nativo a recursos avançados como JSONB, procedures e transações, tornando-o uma excelente escolha para aplicações escaláveis e de alta confiabilidade.

* Docker foi adotado para containerização da aplicação, garantindo um ambiente padronizado, facilitando a execução do projeto em diferentes máquinas e simplificando o processo de deploy.

* React foi utilizado no frontend devido à maior familiaridade com a biblioteca, permitindo desenvolvimento mais rápido, componentização eficiente e melhor organização da interface.

* A Arquitetura Hexagonal foi utilizada com o objetivo de desacoplar as regras de negócio das camadas externas da aplicação, facilitando manutenção, testes e futuras substituições de tecnologias sem impactar o domínio principal.

* O DDD (Domain Driven Design) foi adotado para organizar melhor as responsabilidades do sistema, deixando as regras de negócio mais claras e próximas do contexto do domínio da aplicação. Essa abordagem também contribui para maior escalabilidade e legibilidade do código.

De forma geral, as escolhas priorizam produtividade, previsibilidade e aderência a práticas já consolidadas no desenvolvimento profissional.

---

## Configuração do Banco de Dados

### 1. Crie o banco

```sql
CREATE DATABASE PROSEL_LAPES;
```

### 2. Habilite a extensão pgcrypto

```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;
```

### 3. Execute os scripts na ordem abaixo

> ⚠️ A ordem importa por causa das foreign keys e dependências entre funções.

---

### 3.1 — Tabelas

```sql
CREATE TABLE IF NOT EXISTS usuarios (
    id                  SERIAL          PRIMARY KEY,
    nome                VARCHAR(100)    NOT NULL,
    sobrenome           VARCHAR(100)    NOT NULL,
    senha_hash          TEXT            NOT NULL,
    tipo_usuario        VARCHAR(20)     NOT NULL,
    cnpj_cpf            VARCHAR(14)     NOT NULL,
    data_nascimento     DATE,
    foto_perfil_url     TEXT,
    ativo               BOOLEAN         DEFAULT TRUE,
    ultimo_login        TIMESTAMP,
    dt_hr_criacao       TIMESTAMP       DEFAULT NOW(),
    dt_hr_atualizacao   TIMESTAMP       DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS usuario_email (
    id          SERIAL          PRIMARY KEY,
    usuario_id  INT             NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    email       VARCHAR(150)    NOT NULL,
    principal   BOOLEAN         DEFAULT FALSE,
    dt_criacao  TIMESTAMP       DEFAULT NOW(),

    CONSTRAINT uq_usuario_email UNIQUE (email)
);

CREATE TABLE IF NOT EXISTS usuario_telefone (
    id          SERIAL          PRIMARY KEY,
    usuario_id  INT             NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    telefone    VARCHAR(30)     NOT NULL,
    principal   BOOLEAN         DEFAULT FALSE,
    dt_criacao  TIMESTAMP       DEFAULT NOW(),

    CONSTRAINT uq_usuario_telefone UNIQUE (telefone)
);

CREATE TABLE IF NOT EXISTS log_eventos (
    id          SERIAL          PRIMARY KEY,
    usuario_id  INT             NULL,
    acao        VARCHAR(50),
    status      VARCHAR(20),
    mensagem    TEXT,
    payload     JSONB,
    dt_hr       TIMESTAMP       DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS idempotencia (
    id                  SERIAL          PRIMARY KEY,
    chave_idempotencia  VARCHAR(100)    UNIQUE NOT NULL,
    operacao            VARCHAR(50)     NOT NULL,
    usuario_id          INT             NULL,
    resultado           JSONB           NULL,
    dt_hr_criacao       TIMESTAMP       DEFAULT NOW()
);
```

---

### 3.2 — Função de log

> Deve ser criada **antes** da função principal, pois ela é chamada internamente.

```sql
CREATE OR REPLACE FUNCTION fn_log_evento(
    p_usuario_id    INT,
    p_acao          TEXT,
    p_status        TEXT,
    p_mensagem      TEXT,
    p_msg_in        JSONB,
    p_msg_out       JSONB
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO log_eventos(
        usuario_id,
        acao,
        status,
        mensagem,
        payload
    )
    VALUES (
        p_usuario_id,
        p_acao,
        p_status,
        p_mensagem,
        jsonb_build_object(
            'msgIn',  p_msg_in,
            'msgOut', p_msg_out
        )
    );
EXCEPTION
    WHEN OTHERS THEN
        NULL; -- nunca derruba a operação principal
END;
$$;
```

---

### 3.3 — Função principal de cadastro de usuário

```sql
CREATE OR REPLACE FUNCTION fn_usuario_criar_completo(
    p_nome TEXT,
    p_sobrenome TEXT,
    p_email TEXT,
    p_senha TEXT,
    p_confirma_senha TEXT,
    p_tipo_usuario TEXT,
    p_cnpj_cpf TEXT,
    p_data_nascimento DATE,
    p_chave_idempotencia TEXT,
    p_foto_perfil_url TEXT DEFAULT NULL,
    p_telefone TEXT DEFAULT NULL
)
RETURNS JSONB
LANGUAGE plpgsql
AS $$
DECLARE
    v_usuario_id INT;
    v_hash TEXT;
    v_email_sanitizado TEXT;
    v_cpf_cnpj_sanitizado TEXT;
    v_telefone_sanitizado TEXT;
    v_mensagem_erro TEXT;
    v_chave_existe BOOLEAN;

    -- Auditoria
    v_msg_in JSONB;
    v_msg_out JSONB;

BEGIN

    
    -- MONTA msgIn
    

    v_msg_in := jsonb_build_object(
        'nome', p_nome,
        'sobrenome', p_sobrenome,
        'email', p_email,
        'tipo_usuario', p_tipo_usuario,
        'cnpj_cpf', p_cnpj_cpf,
        'data_nascimento', p_data_nascimento,
        'telefone', p_telefone,
        'chave_idempotencia', p_chave_idempotencia
    );

    -- 2. SANITIZAÇÃO

    v_email_sanitizado :=
        LOWER(TRIM(p_email));

    v_cpf_cnpj_sanitizado :=
        regexp_replace(p_cnpj_cpf, '\D', '', 'g');

    IF p_telefone IS NOT NULL THEN
        v_telefone_sanitizado :=
            regexp_replace(p_telefone, '\D', '', 'g');
    END IF;

    
    -- VALIDAÇÃO CHAVE IDEMPOTÊNCIA
   

    IF COALESCE(TRIM(p_chave_idempotencia), '') = '' THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 400,
                'msgErro', 'Chave de idempotência obrigatória.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            NULL,
            'CREATE_USER',
            'ERROR',
            'Chave de idempotência obrigatória',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    SELECT EXISTS (
        SELECT 1
        FROM idempotencia
        WHERE chave_idempotencia = p_chave_idempotencia
    )
    INTO v_chave_existe;

    IF v_chave_existe THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 409,
                'msgErro', 'Chave de idempotência já utilizada.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            NULL,
            'CREATE_USER',
            'ERROR',
            'Chave de idempotência já utilizada',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    
    -- VALIDAÇÃO SENHA

    IF p_senha <> p_confirma_senha THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 400,
                'msgErro', 'Senha e confirmação de senha não conferem.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            NULL,
            'CREATE_USER',
            'ERROR',
            'Senha e confirmação diferentes',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    -- 5. VALIDAÇÕES
    

    IF v_email_sanitizado !~ '^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$'
    THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 400,
                'msgErro', 'Formato de e-mail inválido.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            NULL,
            'CREATE_USER',
            'ERROR',
            'Formato de e-mail inválido',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    IF length(v_cpf_cnpj_sanitizado) NOT IN (11, 14)
    THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 400,
                'msgErro', 'CPF/CNPJ inválido.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            NULL,
            'CREATE_USER',
            'ERROR',
            'CPF/CNPJ inválido',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    -- HASH SENHA
    

    v_hash :=
        crypt(p_senha, gen_salt('bf'));

    -- INSERT USUARIO
    

    INSERT INTO usuarios (
        nome,
        sobrenome,
        senha_hash,
        tipo_usuario,
        cnpj_cpf,
        data_nascimento,
        foto_perfil_url
    )
    VALUES (
        p_nome,
        p_sobrenome,
        v_hash,
        p_tipo_usuario,
        v_cpf_cnpj_sanitizado,
        p_data_nascimento,
        p_foto_perfil_url
    )
    RETURNING id
    INTO v_usuario_id;

    -- INSERT EMAIL

    INSERT INTO usuario_email(
        usuario_id,
        email,
        principal
    )
    VALUES (
        v_usuario_id,
        v_email_sanitizado,
        TRUE
    );

    -- INSERT TELEFONE

    IF v_telefone_sanitizado IS NOT NULL
       AND v_telefone_sanitizado <> ''
    THEN

        INSERT INTO usuario_telefone(
            usuario_id,
            telefone,
            principal
        )
        VALUES (
            v_usuario_id,
            v_telefone_sanitizado,
            TRUE
        );
    END IF;    

    -- msgOut SUCESSO

    v_msg_out := jsonb_build_object(
        'Status', 0,
        'SuccessObject', jsonb_build_object(
            'id', v_usuario_id,
            'nome', p_nome,
            'sobrenome', p_sobrenome,
            'email', v_email_sanitizado,
            'tipoUsuario', CASE
                               WHEN p_tipo_usuario = 'ADMIN' THEN 0
                               WHEN p_tipo_usuario = 'CUSTOMER' THEN 1
                               ELSE NULL
                           END,
			'telefone',        v_telefone_sanitizado,
	        'cnpjCpf',         v_cpf_cnpj_sanitizado,
	        'dataNascimento',  p_data_nascimento,
	        'fotoPerfilUrl',   p_foto_perfil_url
        )
    );

    -- REGISTRA IDEMPOTÊNCIA

    INSERT INTO idempotencia(
        chave_idempotencia,
        operacao,
        usuario_id,
        resultado
    )
    VALUES(
        p_chave_idempotencia,
        'CREATE_USER',
        v_usuario_id,
        v_msg_out
    );

    -- AUDITORIA SUCESSO

    PERFORM fn_log_evento(
        v_usuario_id,
        'CREATE_USER',
        'SUCCESS',
        'Usuário criado com sucesso',
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;

EXCEPTION

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

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 2,
                'codErro', 409,
                'msgErro', v_mensagem_erro,
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            v_usuario_id,
            'CREATE_USER',
            'ERROR',
            v_mensagem_erro,
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    WHEN OTHERS THEN

        v_msg_out := jsonb_build_object(
            'Status', 2,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 3,
                'codErro', 500,
                'msgErro', 'Erro interno no banco.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            v_usuario_id,
            'CREATE_USER',
            'ERROR',
            'Erro interno no banco: ' || SQLERRM,
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

END;
$$;
```

---

### 3.4 — Tabela de Refresh Token

```sql
CREATE TABLE usuario_refresh_token (
    id SERIAL PRIMARY KEY,
    usuario_id INT NOT NULL,
    refresh_token TEXT NOT NULL,
    expiracao TIMESTAMP NOT NULL,
    revogado BOOLEAN DEFAULT FALSE,
    criado_em TIMESTAMP DEFAULT NOW(),

    CONSTRAINT fk_usuario_refresh_token
        FOREIGN KEY (usuario_id)
        REFERENCES usuarios(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_usuario_refresh_token_usuario_id
ON usuario_refresh_token(usuario_id);
```

---

### 3.5 — Função de Login

```sql
CREATE OR REPLACE FUNCTION fn_usuario_login(
    p_email TEXT,
    p_senha TEXT
)
RETURNS JSONB
LANGUAGE plpgsql
AS $$
DECLARE
    v_usuario_id INT;
    v_senha_hash TEXT;

    v_usuario JSONB;
	
	v_usuario_ativo BOOLEAN;

    v_refresh_token TEXT;
    v_refresh_expiracao TIMESTAMP;

    v_msg_in JSONB;
    v_msg_out JSONB;

BEGIN

-- MSG IN
v_msg_in := jsonb_build_object(
    'email', LOWER(TRIM(p_email))
);

-- BUSCA USUÁRIO
SELECT
    u.id,
    u.senha_hash,
    u.ativo,
    jsonb_build_object(
        'Id', u.id,
        'nome', u.nome,
        'sobrenome', u.sobrenome,
        'email', e.email,
        'tipoUsuario',
        CASE
            WHEN u.tipo_usuario = 'ADMIN' THEN 0
            WHEN u.tipo_usuario = 'CUSTOMER' THEN 1
            ELSE NULL
        END,
        'telefone', t.telefone,
        'cnpjCpf', u.cnpj_cpf,
        'dataNascimento', u.data_nascimento,
        'fotoPerfilUrl', u.foto_perfil_url,
        'Ativo', u.ativo,
        'ultimoLogin', u.ultimo_login,
        'dtHrCriacao', u.dt_hr_criacao,
        'dtHrAtualizacao', u.dt_hr_atualizacao
    )
INTO
    v_usuario_id,
    v_senha_hash,
    v_usuario_ativo,
    v_usuario
FROM usuarios u
JOIN usuario_email e ON e.usuario_id = u.id
LEFT JOIN usuario_telefone t ON t.usuario_id = u.id AND t.principal = TRUE
WHERE e.email = LOWER(TRIM(p_email))
LIMIT 1;

-- USUÁRIO NÃO ENCONTRADO
IF v_usuario_id IS NULL THEN

    v_msg_out := jsonb_build_object(
        'Status', 1,
        'SuccessObject', NULL,
        'ErrorObject', jsonb_build_object(
            'tipoErro', 1,
            'codErro', 401,
            'msgErro', 'Usuário não encontrado',
            'origemErro', 'PostgreSQL'
        )
    );

    PERFORM fn_log_evento(
        NULL,
        'LOGIN',
        'ERROR',
        'Usuário não encontrado',
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;
END IF;

-- VALIDA USUÁRIO ATIVO
IF v_usuario_ativo = FALSE THEN

    v_msg_out := jsonb_build_object(
        'Status', 1,
        'SuccessObject', NULL,
        'ErrorObject', jsonb_build_object(
            'tipoErro', 1,
            'codErro', 403,
            'msgErro', 'Usuário desativado.',
            'origemErro', 'PostgreSQL'
        )
    );

    PERFORM fn_log_evento(
        v_usuario_id,
        'LOGIN',
        'ERROR',
        'Tentativa de login em usuário desativado',
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;

END IF;

-- VALIDA SENHA
IF crypt(p_senha, v_senha_hash) <> v_senha_hash THEN

    v_msg_out := jsonb_build_object(
        'Status', 1,
        'SuccessObject', NULL,
        'ErrorObject', jsonb_build_object(
            'tipoErro', 1,
            'codErro', 401,
            'msgErro', 'Senha inválida',
            'origemErro', 'PostgreSQL'
        )
    );

    PERFORM fn_log_evento(
        v_usuario_id,
        'LOGIN',
        'ERROR',
        'Senha inválida',
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;
END IF;

-- ATUALIZA ÚLTIMO LOGIN
UPDATE usuarios
SET
    ultimo_login = NOW(),
    dt_hr_atualizacao = NOW()
WHERE id = v_usuario_id;

UPDATE usuario_refresh_token
SET revogado = TRUE
WHERE usuario_id = v_usuario_id
  AND revogado = FALSE;

-- REVOGA TOKENS ANTIGOS

-- GERA REFRESH TOKEN
v_refresh_token := encode(gen_random_bytes(64), 'hex');
v_refresh_expiracao := NOW() + INTERVAL '7 days';

INSERT INTO usuario_refresh_token(
    usuario_id,
    refresh_token,
    expiracao,
    revogado,
    criado_em
)
VALUES (
    v_usuario_id,
    v_refresh_token,
    v_refresh_expiracao,
    FALSE,
    NOW()
);

-- SUCCESS RESPONSE
v_msg_out := jsonb_build_object(
    'Status', 0,
    'SuccessObject', jsonb_build_object(
        'usuario', v_usuario,
        'refreshToken', v_refresh_token,
        'expiracaoRefreshToken', v_refresh_expiracao
    ),
    'ErrorObject', NULL
);

-- LOG SUCCESS
PERFORM fn_log_evento(
    v_usuario_id,
    'LOGIN',
    'SUCCESS',
    'Login realizado com sucesso',
    v_msg_in,
    v_msg_out
);

RETURN v_msg_out;

-- EXCEPTION
EXCEPTION
WHEN OTHERS THEN

    v_msg_out := jsonb_build_object(
        'Status', 2,
        'SuccessObject', NULL,
        'ErrorObject', jsonb_build_object(
            'tipoErro', 3,
            'codErro', 500,
            'msgErro', 'Erro interno no login',
            'origemErro', 'PostgreSQL'
        )
    );

    PERFORM fn_log_evento(
        NULL,
        'LOGIN',
        'ERROR',
        SQLERRM,
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;
END;
$$;
```

---

### 3.6 — Função para desativar usuário

```sql
CREATE OR REPLACE FUNCTION fn_usuario_desativar(
    p_usuario_id INT
)
RETURNS JSONB
LANGUAGE plpgsql
AS $$
DECLARE

    v_usuario_existe BOOLEAN;

    v_msg_in JSONB;
    v_msg_out JSONB;

BEGIN

    -- MSG IN

    v_msg_in := jsonb_build_object(
        'usuarioId', p_usuario_id
    );

    -- VALIDAÇÃO

    SELECT EXISTS(
        SELECT 1
        FROM usuarios
        WHERE id = p_usuario_id
    )
    INTO v_usuario_existe;

    IF NOT v_usuario_existe THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'SuccessObject', NULL,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 1,
                'codErro', 404,
                'msgErro', 'Usuário não encontrado.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            p_usuario_id,
            'DELETE_USER',
            'ERROR',
            'Usuário não encontrado',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    -- VERIFICA SE JÁ ESTÁ INATIVO

    IF EXISTS(
        SELECT 1
        FROM usuarios
        WHERE id = p_usuario_id
          AND ativo = FALSE
    ) THEN

        v_msg_out := jsonb_build_object(
            'Status', 1,
            'SuccessObject', NULL,
            'ErrorObject', jsonb_build_object(
                'tipoErro', 1,
                'codErro', 409,
                'msgErro', 'Usuário já está desativado.',
                'origemErro', 'PostgreSQL'
            )
        );

        PERFORM fn_log_evento(
            p_usuario_id,
            'DELETE_USER',
            'ERROR',
            'Usuário já estava desativado',
            v_msg_in,
            v_msg_out
        );

        RETURN v_msg_out;

    END IF;

    -- DESATIVA USUÁRIO

    UPDATE usuarios
    SET
        ativo = FALSE,
        dt_hr_atualizacao = NOW()
    WHERE id = p_usuario_id;

    -- REVOGA REFRESH TOKENS

    UPDATE usuario_refresh_token
    SET revogado = TRUE
    WHERE usuario_id = p_usuario_id
      AND revogado = FALSE;

    -- MSG OUT SUCESSO

    v_msg_out := jsonb_build_object(
        'Status', 0,
        'SuccessObject', jsonb_build_object(
            'usuarioId', p_usuario_id,
            'ativo', FALSE,
            'dataDesativacao', NOW()
        ),
        'ErrorObject', NULL
    );

    -- LOG SUCCESS

    PERFORM fn_log_evento(
        p_usuario_id,
        'DELETE_USER',
        'SUCCESS',
        'Usuário desativado com sucesso',
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;

-- EXCEPTION

EXCEPTION
WHEN OTHERS THEN

    v_msg_out := jsonb_build_object(
        'Status', 2,
        'SuccessObject', NULL,
        'ErrorObject', jsonb_build_object(
            'tipoErro', 2,
            'codErro', 500,
            'msgErro', 'Erro interno ao desativar usuário.',
            'origemErro', 'PostgreSQL'
        )
    );

    PERFORM fn_log_evento(
        p_usuario_id,
        'DELETE_USER',
        'ERROR',
        SQLERRM,
        v_msg_in,
        v_msg_out
    );

    RETURN v_msg_out;

END;
$$;
```

---

### 3.7 — Consultas de Validação

```sql
SELECT * FROM usuarios;
SELECT * FROM usuario_email;
```

## 📌 Observações

Este projeto foi desenvolvido como parte do processo seletivo do LAPES 2026.
