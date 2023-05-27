using Microsoft.Build.Framework;

namespace UniversityAccessControl.Auth;

public class JwtOptions
{
    [Required] public required string Issuer { get; set; }
    [Required] public required string Audience { get; set; }
    [Required] public required string Subject { get; set; }
    [Required] public required byte[] Key { get; set; }
}