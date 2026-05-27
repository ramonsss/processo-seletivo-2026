using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Dapper;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Implementations;

public class CadastrarUsuarioRepository(IPostgreSQLConnection connection) : ICadastrarUsuarioRepository
{
    private readonly IPostgreSQLConnection _connection = connection;
    
    public async ValueTask<UsuarioResponse> CadastrarUsuario(UsuarioRequest usuarioRequest)
    { 
        using var activity = Activity.Current?.Source.StartActivity("cadastra-usuario-repository");
        using var connection = _connection.ConnectCLUST("PROSEL_LAPES");

        string functionName = "fn_usuario_criar_completo";
        
        activity?.SetTag("procedure_name", functionName);

        var parameters = new DynamicParameters();
        
        parameters.Add("p_nome", usuarioRequest.nome, DbType.String, ParameterDirection.Input);
        parameters.Add("p_sobrenome", usuarioRequest.sobrenome, DbType.String, ParameterDirection.Input);
        parameters.Add("p_email", usuarioRequest.email, DbType.String, ParameterDirection.Input);
        parameters.Add("p_tipo_usuario", usuarioRequest.tipoUsuario, DbType.String, ParameterDirection.Input);
        parameters.Add("p_cnpj_cpf", usuarioRequest.cnpjCpf, DbType.String, ParameterDirection.Input);
        parameters.Add("p_data_nascimento", usuarioRequest.dataNascimento, DbType.Date, ParameterDirection.Input);
        parameters.Add("p_foto_perfil_url", usuarioRequest.fotoPerfilUrl, DbType.String, ParameterDirection.Input);
        parameters.Add("p_telefone", usuarioRequest.telefone, DbType.String, ParameterDirection.Input);
        
        activity?.SetTag("mensagem_in", JsonSerializer.Serialize(usuarioRequest));
        
        await connection.ExecuteAsync(functionName, parameters, commandType: CommandType.StoredProcedure);
        
        //TODO: continuar a logica de conexão
    }
}