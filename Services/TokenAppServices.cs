using APICadastro.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICadastro.Services;

public class TokenAppServices
{
    private readonly static string stringKey = "af6sl876fuwm5abz5wmbhbkyg23udigyt7dncgzs7vvx4bsgk0";
    public static string GenerateToken(Usuario usuario)
    {
        string tipoAcesso;
        if (usuario.TipoAcessoId == 1)
        {
            tipoAcesso = "Admin";
        }
        else
        {
            tipoAcesso = "Company";
        }

        var claims = new List<Claim>
        {
            new Claim("id", usuario.UsuarioId.ToString()),
            new Claim("acesso", tipoAcesso),
            new Claim("nome", usuario.Nome)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(stringKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: claims,
                    signingCredentials: creds
                    );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static object? ValidateToken(string token)
    {
        if (token == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(stringKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return new
            {
                UserId = jwtToken.Claims.First(x => x.Type == "id").Value,
                Name = jwtToken.Claims.First(x => x.Type == "nome").Value,
                Role = jwtToken.Claims.First(x => x.Type == "acesso").Value
            };
        }
        catch
        {
            return null;
        }
    }
}
