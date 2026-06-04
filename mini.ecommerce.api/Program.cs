using mini.ecommerce.api.Infra.Configuration.Domain;
using mini.ecommerce.api.Infra.Configuration.Inbound;
using mini.ecommerce.api.Infra.Outbound;
using System.Reflection;
using System.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInboundAdapters(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfigs();
builder.Services.AddJwtBearer();
builder.Services.AddFlatValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddUseCaseExtensions();
builder.Services.AddSqlExtensions();

var app = builder.Build();
app.MapSwagger();
app.AddEndpointHttp();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();