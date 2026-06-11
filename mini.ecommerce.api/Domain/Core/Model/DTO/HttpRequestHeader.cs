namespace mini.ecommerce.api.Domain.Core.Model.DTO
{
    public record HttpRequestHeader
    {
        public string? chaveIdempotencia { get; set; }
    }
}