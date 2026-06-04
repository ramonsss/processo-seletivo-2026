using System.Validation;
using mini.ecommerce.api.Domain.Core.Model.VM.Response;

namespace mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Filter;

public class ValidationFilter<T>(IServiceProvider serviceProvider) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        IFlatValidator<T> validator = serviceProvider.GetRequiredService<IFlatValidator<T>>();

        if (context.Arguments.FirstOrDefault(x =>
                x?.GetType() == typeof(T)) is not T model)
        {
            return TypedResults.Problem(
                detail: "Invalid parameter.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        var result = await validator.ValidateAsync(model);
        if (!result)
        {
            ErroResponse erroResponse = new("400", "Requisição inválida");

            foreach (var error in result.Errors)
            {
                erroResponse.erros!.Add(new Erros(error.PropertyName, [error.ErrorMessage]));
            }

            return TypedResults.BadRequest(erroResponse);

        }

        return await next(context);
    }
}