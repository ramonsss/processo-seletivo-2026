using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using System.Validation;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Validation
{
    public class UsuarioValidator : FlatValidator<UsuarioRequest>
    {
        public UsuarioValidator() 
        {
            ErrorIf(r => r.nome is null, r => "Não pode ser nulo", r => r.nome);

            ErrorIf(r => r.email is null, r => "Não pode ser nulo", r => r.email);
            When(r => r.email is not null, @then =>
            {
                ValidIf(r => MyRegex.ValidaEmail(r.email!), r => "Email fora do padrão", r => r.email);
            });

            ErrorIf(r => r.senha is null, r => "Não pode ser nulo", r => r.senha);

            ErrorIf(r => r.dataCriacao is null, r => "Não pode ser nulo", r => r.dataCriacao);

            ErrorIf(r => r.dataAtualizacao is null, r => "Não pode ser nulo", r => r.dataAtualizacao);
        }
    }
}
