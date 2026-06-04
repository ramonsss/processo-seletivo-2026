using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Implementations;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Interfaces;
using mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;

namespace mini.ecommerce.api.Infra.Outbound
{
    public static class SqlExtensions
    {
        public static void AddSqlExtensions(this IServiceCollection services)
        {
            services.AddSingleton<IPostgreSQLConnection, PostgreSQLConnection>();

            services.AddScoped<ICadastrarUsuarioRepository, CadastrarUsuarioRepository>();
            services.AddScoped<ILoginUsuarioRepository, LoginUsuarioRepository>();
        }
    }
}
