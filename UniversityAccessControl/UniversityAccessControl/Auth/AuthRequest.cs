using System.ComponentModel.DataAnnotations;

namespace UniversityAccessControl.Auth;

public sealed class AuthRequest
{
    [Required] public required string UserName { get; set; }
    [Required] public required string Password { get; set; }
}