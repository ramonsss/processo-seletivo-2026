using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Response;

public class DesativarUsuarioFunctionResponse
{
    public EnumStatus Status { get; set; }

    public DesativarUsuarioResponse? SuccessObject { get; set; }

    public BaseError? ErrorObject { get; set; }
}