using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Model.DTO;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Domain.UseCase.Interfaces
{
    public interface ICadastrarUsuarioUseCase
    {
        Task<BaseReturn<UsuarioResponse>> CadastraUsuario(UsuarioRequest domainRequest);
    }
}
