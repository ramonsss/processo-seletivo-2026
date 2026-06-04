using mini.ecommerce.api.Domain.UseCase.Interfaces;
using mini.ecommerce.api.Domain.UseCases.Implementations;

namespace mini.ecommerce.api.Infra.Configuration.Domain
{
    public static class UseCaseExtensions
    {
        public static void AddUseCaseExtensions(this IServiceCollection services)
        {
            services.AddScoped<ICadastrarUsuarioUseCase, CadastrarUsuarioUseCase>();
            services.AddScoped<ILoginUsuarioUsecase, LoginUsuarioUsecase>();
        }
    }
}
