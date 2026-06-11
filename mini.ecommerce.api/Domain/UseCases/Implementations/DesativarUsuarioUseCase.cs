using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Enums;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;
using mini.ecommerce.api.Domain.UseCase.Interfaces;

namespace mini.ecommerce.api.Domain.UseCases.Implementations;

public class DesativarUsuarioUseCase(IServiceProvider serviceProvider) : IDesativarUsuarioUseCase
{
    private readonly IDesativarUsuarioRepository _desativarUsuarioRepository = serviceProvider.GetRequiredService<IDesativarUsuarioRepository>();

    public async Task<BaseReturn<DesativarUsuarioResponse>> DesativarUsuarioAsync(int usuarioId)
    {
        var response = await _desativarUsuarioRepository.DesativarUsuario(usuarioId);

        if (response.Status == EnumStatus.SUCESSO)
        {
            return BaseReturn<DesativarUsuarioResponse>.Success(response.SuccessObject!);
        }

        return BaseReturn<DesativarUsuarioResponse>.Error(response.Status, response.ErrorObject!);
    }
}