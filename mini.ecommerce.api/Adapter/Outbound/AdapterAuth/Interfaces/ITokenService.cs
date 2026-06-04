namespace mini.ecommerce.api.Adapter.Outbound.AdapterAuth.Interfaces;

public interface ITokenService
{
    string GenerateToken(string email, string role);
}