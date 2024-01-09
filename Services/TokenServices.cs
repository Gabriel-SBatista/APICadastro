using APICadastro.Models;
using APICadastro.Repositories;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICadastro.Services;

public class TokenServices
{
    private readonly static string stringKey = "af6sl876fuwm5abz5wmbhbkyg23udigyt7dncgzs7vvx4bsgk0";
    private readonly AccessTypeRepository _accessTypeRepository;
    private readonly CompanyRepository _companyRepository;

    public TokenServices(AccessTypeRepository accessTypeRepository, CompanyRepository companyRepository)
    {
        _accessTypeRepository = accessTypeRepository;
        _companyRepository = companyRepository;
    }
    public async Task<string> GenerateToken(User user)
    {
        var accessType = await _accessTypeRepository.GetById(ObjectId.Parse(user.AccessTypeId));
        Company? company = null;

        if(user.CompanyId != null)
        {
            company = await _companyRepository.GetById(ObjectId.Parse(user.CompanyId));
        }      

        var claims = new List<Claim>
        {
            new Claim("id", user.UserId.ToString()),
            new Claim("access", accessType.Type),
            new Claim("name", user.Name),
            new Claim("company", company.Name)
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

    public object? ValidateToken(string token)
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
                Name = jwtToken.Claims.First(x => x.Type == "name").Value,
                Role = jwtToken.Claims.First(x => x.Type == "access").Value,
                Company = jwtToken.Claims.First(x => x.Type == "company").Value
            };
        }
        catch
        {
            return null;
        }
    }
}
