using mini.ecommerce.api.Domain.Core.Enums;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Response
{
    public record UsuarioResponse
    {
        public int? id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public EnumTipoUsuario? tipoUsuario { get; set; }
        public DateTime? dataCriacao { get; set; }
        public DateTime? dataAtualizacao { get; set; }
    }
}
