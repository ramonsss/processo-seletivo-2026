using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Request
{
    public record UsuarioRequest
    {
        public string? nome { get; set; }
        public string? sobrenome { get; set; }
        public string? email { get; set; }
        public string? senha { get; set; }
        public string? confirmaSenha { get; set; }
        public EnumTipoUsuario? tipoUsuario { get; set; }
        public string? telefone { get; set; }
        public string? cnpjCpf { get; set; }
        public DateOnly? dataNascimento { get; set; }
        public string? fotoPerfilUrl { get; set; }
        public bool? Ativo { get; set; }
        public DateOnly? UltimoLogin { get; set; }
    }
}
