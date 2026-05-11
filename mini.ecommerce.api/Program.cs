using mini.ecommerce.api.Infra.Configuration.Inbound;
using System.Reflection;
using System.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInboundAdapters(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfigs();
builder.Services.AddJwtBearer();
//builder.Services.AddDomainServices(); -> Vai ser pros useCases
//builder.Services.AddSQLConfig(builder.Configuration/*, ["SQLCLUST05"]*/); -> vai ser pro banco

var app = builder.Build();
app.MapSwagger();
app.AddEndpointHttp();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();