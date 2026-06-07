using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Dapper;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Implementations;

public class DesativarUsuarioRepository(IPostgreSQLConnection connection) : IDesativarUsuarioRepository
{
    private readonly IPostgreSQLConnection _connection = connection;
    
    public async ValueTask<DesativarUsuarioFunctionResponse> DesativarUsuario(int usuarioId)
    {
        using var activity = Activity.Current?.Source.StartActivity("desativar-usuario-repository");
        using var connection = _connection.ConnectCLUST("PROSEL_LAPES");
        
        string functionName = "fn_usuario_desativar";
        
        activity?.SetTag("procedure_name", functionName);

        var parameters = new DynamicParameters();
        
        parameters.Add("p_usuario_id", usuarioId, DbType.Int32, ParameterDirection.Input);
        
        activity?.SetTag("mensagem_in", JsonSerializer.Serialize(new { usuarioId }));
        
        var sql =
            $"SELECT {functionName}(" +
            "@p_usuario_id" +
            ");";

        var jsonResponse = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);

        if (string.IsNullOrWhiteSpace(jsonResponse))
        {
            throw new Exception(
                "Nenhuma resposta retornada pela função.");
        }

        activity?.SetTag("mensagem_out", jsonResponse);

        var response =
            JsonSerializer.Deserialize<
                DesativarUsuarioFunctionResponse>(
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