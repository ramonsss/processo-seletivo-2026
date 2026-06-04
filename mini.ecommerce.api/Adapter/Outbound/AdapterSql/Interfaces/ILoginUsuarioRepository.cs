using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;

public interface ILoginUsuarioRepository
{
    public ValueTask<LoginFunctionResponse> LoginUsuario(LoginRequest loginRequest);
}