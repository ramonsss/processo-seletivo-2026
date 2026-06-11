namespace mini.ecommerce.api.Domain.Core.Model.VM.Request
{
    public record AlterarSenhaUsuarioRequest
    {
        public int? id { get; set; }
        public string? senhaAtual { get; set; }
        public string? novaSenha { get; set; }
        public string? confirmaNovaSenha { get; set; }
    }
}
