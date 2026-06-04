using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Filter;
using mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Mapper;
using mini.ecommerce.api.Domain.Core.Enums;
using mini.ecommerce.api.Domain.Core.Model.DTO;
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
            .AddEndpointFilter<ValidationFilter<UsuarioRequest>>()
            //.RequireAuthorization()
            ;

        }

        public static async Task<IResult> CadastraUsuario([FromBody] UsuarioRequest request,
                                                          [FromHeader(Name = "Chave-Idempotencia")] string? chaveIdempotencia,
                                                          [FromServices] ICadastrarUsuarioUseCase useCase)
        {
            chaveIdempotencia ??= Guid.NewGuid().ToString();

            request.header ??= new HttpRequestHeader();

            request.header.chaveIdempotencia = chaveIdempotencia;
            
            request.header!.chaveIdempotencia = chaveIdempotencia;
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
