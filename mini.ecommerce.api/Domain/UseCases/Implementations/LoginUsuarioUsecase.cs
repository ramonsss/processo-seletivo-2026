using mini.ecommerce.api.Adapter.Outbound.AdapterAuth.Interfaces;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Enums;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;
using mini.ecommerce.api.Domain.UseCase.Interfaces;

namespace mini.ecommerce.api.Domain.UseCases.Implementations;

public class LoginUsuarioUsecase(IServiceProvider serviceProvider) : ILoginUsuarioUsecase
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = serviceProvider.GetRequiredService<ILoginUsuarioRepository>();
    private readonly ITokenService _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    
    public async Task<BaseReturn<LoginResponse>> LoginUsuarioAsync(LoginRequest domainRequest)
    {
        LoginFunctionResponse response = await _loginUsuarioRepository.LoginUsuario(domainRequest);
        
        if (response.Status != EnumStatus.SUCESSO)
        {
            return BaseReturn<LoginResponse>.Error(
                response.Status,
                response.ErrorObject
            );
        }

        var usuario = response.SuccessObject.usuario;

        var token = _tokenService.GenerateToken(
            usuario.Id!.Value,
            usuario.email!,
            usuario.tipoUsuario!.ToString()
        );

        return BaseReturn<LoginResponse>.Success(new LoginResponse
        {
            token = token,
            refreshToken = response.SuccessObject.refreshToken,
            expiracaoToken = DateTime.UtcNow.AddHours(2),
            usuario = usuario
        });
    }
}