using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Response
{
    public record UsuarioResponse
    {
        public int? Id { get; set; }
        public string? nome { get; set; }
        public string? sobrenome { get; set; }
        public string? email { get; set; }
        public EnumTipoUsuario? tipoUsuario { get; set; }
        public string? telefone { get; set; }
        public string? cnpjCpf { get; set; }
        public DateOnly? dataNascimento { get; set; }
        public string? fotoPerfilUrl { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime? dtHrCriacao { get; set; }
        public DateTime? dtHrAtualizacao { get; set; }
    }
}
