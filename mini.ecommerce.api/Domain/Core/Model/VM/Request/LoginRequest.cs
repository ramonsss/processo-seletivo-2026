namespace mini.ecommerce.api.Domain.Core.Model.VM.Request
{
    public record LoginRequest
    {
        public string? email { get; set; }
        public string? senha { get; set; }
    }
}
