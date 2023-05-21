using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace UniversityAccessControl.Auth;

public sealed class JwtService
{
    private const int ExpirationMinutes = 30;

    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResponse CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new AuthResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration)
    {
        return new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private IEnumerable<Claim> CreateClaims(IdentityUser user)
    {
        var subject = _configuration["Jwt:Subject"];
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(user.UserName);

        return new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var key = _configuration["Jwt:Key"];
        ArgumentNullException.ThrowIfNull(key);

        return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
    }
}