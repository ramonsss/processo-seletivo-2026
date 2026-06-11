namespace mini.ecommerce.api.Domain.Core.Model.VM.Response
{
    public class LoginResponse
    {
        public string? token { get; set; }
        public string? refreshToken { get; set; }
        public DateTime? expiracaoToken { get; set; }
        public UsuarioResponse? usuario { get; set; }
    }
}
