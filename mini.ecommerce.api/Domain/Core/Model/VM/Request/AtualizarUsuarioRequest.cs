namespace mini.ecommerce.api.Domain.Core.Model.VM.Request
{
    public record AtualizarUsuarioRequest
    {
        public int? id { get; set; }
        public string? nome { get; set; }
        public string? sobrenome { get; set; }
        public string? email { get; set; }
        public string? telefone { get; set; }
        public string? cnpjCpf { get; set; }
        public DateOnly? dataNascimento { get; set; }
        public string? fotoPerfilUrl { get; set; }
    }
}
