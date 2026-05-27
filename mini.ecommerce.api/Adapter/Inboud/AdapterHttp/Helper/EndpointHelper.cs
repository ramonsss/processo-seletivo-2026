using System.Diagnostics;
using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Mapper
{
    public static class EndpointHelper
    {
        //public static IResult ProcessUseCaseTransactionResult<U>(BaseReturn<U> result) where U : class
        //{
        //    return result.Status != EnumStatus.SUCESSO
        //        ? MappingToEndpoint.MappingErrorToEndpoint(result.ErrorObject!)
        //        : Results.Ok(result.SuccessObject);
        //}

        public static IResult HandleException(Exception ex, string activityName = "Erro")
        {
            using var activity = Activity.Current?.Source.StartActivity(activityName);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.SetTag("Stack Trace", ex.StackTrace);

            var error = new BaseError(EnumStatus.SISTEMA, 500, ex.Message);
            return Results.Json(error, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
