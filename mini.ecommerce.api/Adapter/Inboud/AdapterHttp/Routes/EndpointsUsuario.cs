using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Routes
{
    public static class EndpointsUsuario
    {
        public static void AddEndpointAcessoUsuario(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/teste", Teste);

        }

        public static async Task<IResult> Teste()
        {
            return Results.Ok();
        }
    }
}
