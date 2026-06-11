using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;

public interface IDesativarUsuarioRepository
{
    public ValueTask<DesativarUsuarioFunctionResponse> DesativarUsuario(int usuarioId);
}