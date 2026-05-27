using mini.ecommerce.api.Domain.Core.Base;
using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Response
{
    public class UsuarioFunctionResponse
    {
        public EnumStatus Status { get; set; }

        public UsuarioResponse? SuccessObject { get; set; }

        public BaseError? ErrorObject { get; set; }
    }
}
