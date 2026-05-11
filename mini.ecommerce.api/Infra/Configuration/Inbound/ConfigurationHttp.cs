using mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Routes;

namespace mini.ecommerce.api.Infra.Configuration.Inbound
{
    public static class ConfigurationHttp
    {
        public static void AddEndpointHttp(this IEndpointRouteBuilder app)
        {
            app.AddEndpointAcessoUsuario();
        }

        public static IServiceCollection ConfigureInboundAdapters(this IServiceCollection services, IConfiguration configuration)
        {
            // CORS Configuration
            services.AddCors(options =>
            {
                var allowedOrigins = configuration
                .GetSection("AppSettings:Cors:AllowedOrigins")
                .Get<string[]>();

                options.AddPolicy("ApiCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .WithMethods("POST", "OPTIONS")
                        .WithHeaders("Authorization",
                                     "Content-Type",
                                     "codUsuario")
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
