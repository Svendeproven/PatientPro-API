using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exam_api_project.models.Entities;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;
using Microsoft.IdentityModel.Tokens;

namespace exam_api_project.Services.Security;

public class TokenService : ITokenService
{
    public string GenerateToken(UserModel user)
    {
        var jwtSecret =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
        var credentials = new SigningCredentials(jwtSecret, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var token = new JwtSecurityToken(
            "exam-api",
            claims: claims,
            expires: DateTime.Now.AddDays(365),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}