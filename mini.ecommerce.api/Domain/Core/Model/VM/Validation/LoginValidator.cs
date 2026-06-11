using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using System.Validation;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Validation
{
    public class LoginValidator : FlatValidator<LoginRequest>
    {
        public LoginValidator()
        {
            ErrorIf(r => r.email is null, r => "Não pode ser nulo", r => r.email);

            ErrorIf(r => r.senha is null, r => "Não pode ser nulo", r => r.senha);
        }
    }
}
