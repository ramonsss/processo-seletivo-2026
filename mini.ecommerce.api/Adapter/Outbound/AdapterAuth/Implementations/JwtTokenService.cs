using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using mini.ecommerce.api.Adapter.Outbound.AdapterAuth.Interfaces;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterAuth.Implementations;

public class JwtTokenService : ITokenService
{
    private readonly byte[] _key;

    public JwtTokenService()
    {
        _key = Encoding.ASCII.GetBytes("dAWG7KP2xpHPN8aU1GfC82OkOqwXSz5w");
    }

    public string GenerateToken(int usuarioId, string email, string role)
    {
        var handler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),

            Expires = DateTime.UtcNow.AddHours(2),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}