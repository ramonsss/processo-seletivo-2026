using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
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
            var response = await useCase.CadastraUsuarioAsync(request);


            if (response.Status == EnumStatus.SUCESSO)
            {
                return Results.Ok(response);
            }

            if (response.Status == EnumStatus.NEGOCIO)
            {
                return Results.BadRequest(response);
            }

            return Results.Problem(
                detail: response.ErrorObject?.msgErro,
                statusCode: 500
            );
        }
    }
}
