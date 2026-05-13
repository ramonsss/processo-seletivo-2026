using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace mini.ecommerce.api.Infra.Configuration.Inbound
{
    public static class ServiceSwagger
    {
        public static void AddSwaggerConfigs(this IServiceCollection service)
        {
            service.AddSwaggerGen(ConfigureSwaggerGenOptions);
        }

        public static void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            var contact = new OpenApiContact();
            contact.Name = "Ramon Souza & Yslan Lopes";
            contact.Email = "ramon14souza@gmail.com & yslan.contato@gmail.com";

            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "",
            };

            var securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            };

            var openApiInfo = new OpenApiInfo();
            openApiInfo.Version = "v1";
            openApiInfo.Title = "mini.ecommerce.api";
            openApiInfo.Description = "API para processo seletivo LAPES";
            openApiInfo.Contact = contact;

            options.SwaggerDoc("v1", openApiInfo);
            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        }

        public static WebApplication MapSwagger(this WebApplication app)
        {
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
