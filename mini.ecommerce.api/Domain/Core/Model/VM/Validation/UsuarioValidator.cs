using mini.ecommerce.api.Domain.Core.Model.VM.Request;
using System.Validation;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Validation
{
    public class UsuarioValidator : FlatValidator<UsuarioRequest>
    {
        public UsuarioValidator() 
        {
            ErrorIf(r => r.nome is null, r => "Não pode ser nulo", r => r.nome);

            ErrorIf(r => r.sobrenome is null, r => "Não pode ser nulo", r => r.sobrenome);

            ErrorIf(r => r.email is null, r => "Não pode ser nulo", r => r.email);
            When(r => r.email is not null, @then =>
            {
                ValidIf(r => MyRegex.ValidaEmail(r.email!), r => "Email fora do padrão", r => r.email);
            });

            ErrorIf(r => r.senha is null, r => "Não pode ser nulo", r => r.senha);

            ErrorIf(r => r.confirmaSenha is null, r => "Não pode ser nulo", r => r.confirmaSenha);

            ErrorIf(r => r.tipoUsuario is null, r => "Não pode ser nulo", r => r.tipoUsuario);

            ErrorIf(r => r.telefone is null, r => "Não pode ser nulo", r => r.telefone);

            ErrorIf(r => r.cnpjCpf is null, r => "Não pode ser nulo", r => r.cnpjCpf);
            When(r => r.email is not null, @then =>
            {
                ValidIf(r => MyRegex.ValidaCpfCnpj(r.cnpjCpf!), r => "CPF/CNPJ fora do padrão", r => r.cnpjCpf);
            });

            ErrorIf(r => r.dataNascimento is null, r => "Não pode ser nulo", r => r.dataNascimento);
        }
    }
}
