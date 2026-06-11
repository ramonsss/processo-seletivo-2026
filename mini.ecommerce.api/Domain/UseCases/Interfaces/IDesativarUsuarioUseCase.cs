using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Domain.UseCase.Interfaces;

public interface IDesativarUsuarioUseCase
{
    Task<BaseReturn<DesativarUsuarioResponse>> DesativarUsuarioAsync(int usuarioId);
}