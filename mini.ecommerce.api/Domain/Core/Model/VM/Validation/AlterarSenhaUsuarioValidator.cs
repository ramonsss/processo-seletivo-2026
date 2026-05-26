using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using System.Validation;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Validation
{
    public class AlterarSenhaUsuarioValidator : FlatValidator<AlterarSenhaUsuarioRequest>
    {
        public AlterarSenhaUsuarioValidator()
        {
            ErrorIf(r => r.id is null, r => "Não pode ser nulo", r => r.id);

            ErrorIf(r => r.senhaAtual is null, r => "Não pode ser nulo", r => r.senhaAtual);

            ErrorIf(r => r.novaSenha is null, r => "Não pode ser nulo", r => r.novaSenha);

            ErrorIf(r => r.confirmaNovaSenha is null, r => "Não pode ser nulo", r => r.confirmaNovaSenha);
        }
    }
}
