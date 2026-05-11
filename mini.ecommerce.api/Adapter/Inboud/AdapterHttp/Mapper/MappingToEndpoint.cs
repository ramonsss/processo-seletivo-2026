using apiMcardPrePagoGestorRelatorio.Domain.Core.Base;
using apiMcardPrePagoGestorRelatorio.Domain.Core.Enums;

namespace mini.ecommerce.api.Adapter.Inboud.AdapterHttp.Mapper
{
    public class MappingToEndpoint
    {
        public static IResult MappingErrorToEndpoint(BaseError error)
        {
            return error!.tipoErro == EnumStatus.SISTEMA ? Results.Json(error, statusCode: StatusCodes.Status500InternalServerError) : Results.BadRequest(error);
        }
    }
}
