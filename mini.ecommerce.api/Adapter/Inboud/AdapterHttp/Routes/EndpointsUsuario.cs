using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Mapper;
using mini.ecommerce.api.Domain.Core.Enums;
using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using mini.ecommerce.api.Domain.UseCase.Interfaces;

namespace mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Routes
{
    public static class EndpointsUsuario
    {
        public static void AddEndpointAcessoUsuario(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/usuario", CadastraUsuario)
            .WithTags("Cadastrar Usuarios")
            .RequireAuthorization()
            ;

        }

        public static async Task<IResult> CadastraUsuario([FromBody] UsuarioRequest request,
                                                          [FromServices] ICadastrarUsuarioUseCase useCase)
        {
            try
            {
                var response = await useCase.CadastraUsuarioAsync(request);

                return response.Status ==
                       Domain.Core.Enums.EnumStatus.SUCESSO
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            }
            catch (Exception ex)
            {
                return EndpointHelper
                    .HandleException(
                        ex,
                        "cadastrar-usuario-endpoint");
            }
        }
    }
}
