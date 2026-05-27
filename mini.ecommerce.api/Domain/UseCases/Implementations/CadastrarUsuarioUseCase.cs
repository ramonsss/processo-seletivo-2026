using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Model.DTO;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;
using mini.ecommerce.api.Domain.UseCase.Interfaces;
using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.UseCases.Implementations
{
    public class CadastrarUsuarioUseCase(IServiceProvider serviceProvider) : ICadastrarUsuarioUseCase
    {
        private readonly ICadastrarUsuarioRepository _cadastrarUsuarioRepository = serviceProvider.GetRequiredService<ICadastrarUsuarioRepository>();
        
        public async Task<BaseReturn<UsuarioResponse>> CadastraUsuarioAsync(UsuarioRequest domainRequest)
        {
            UsuarioFunctionResponse response = await _cadastrarUsuarioRepository.CadastrarUsuario(domainRequest);

            if(response.Status == EnumStatus.SUCESSO)
                return BaseReturn<UsuarioResponse>.Success(response.SuccessObject!);

            return BaseReturn<UsuarioResponse>.Error(response.Status, response.ErrorObject!);
        }
    }
}
