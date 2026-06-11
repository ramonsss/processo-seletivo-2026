namespace mini.ecommerce.api.Domain.Core.Model.VM.Response;

public class DesativarUsuarioResponse
{
    public int? usuarioId { get; set; }
    public bool? ativo { get; set; }
    public DateTime? dataDesativacao { get; set; }
}