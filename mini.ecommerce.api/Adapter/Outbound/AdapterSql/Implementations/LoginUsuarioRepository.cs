using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Dapper;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Implementations;

public class LoginUsuarioRepository(IPostgreSQLConnection connection) : ILoginUsuarioRepository
{
    private readonly IPostgreSQLConnection _connection = connection;
    
    public async ValueTask<LoginFunctionResponse> LoginUsuario(LoginRequest loginRequest)
    {
        using var activity = Activity.Current?.Source.StartActivity("login-usuario-repository");
        using var connection = _connection.ConnectCLUST("PROSEL_LAPES");
        
        string functionName = "fn_usuario_login";
        
        activity?.SetTag("procedure_name", functionName);

        var parameters = new DynamicParameters();
        
        parameters.Add("p_email", loginRequest.email, DbType.String, ParameterDirection.Input);
        parameters.Add("p_senha", loginRequest.senha, DbType.String, ParameterDirection.Input);
        
        activity?.SetTag("mensagem_in", JsonSerializer.Serialize(loginRequest));
        
        var sql =
            $"SELECT {functionName}(" +
            "@p_email," +
            "@p_senha" +
            ");";
        
        var jsonResponse = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);

        if (string.IsNullOrWhiteSpace(jsonResponse))
        {
            throw new Exception("Nenhuma resposta retornada pela função.");
        }

        activity?.SetTag("mensagem_out", jsonResponse);

        var response = JsonSerializer.Deserialize<LoginFunctionResponse>(
            jsonResponse,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (response is null) 
        {
            throw new Exception(
                "Erro ao desserializar resposta.");
        }

        return response;
    }
}