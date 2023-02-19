using System.ComponentModel.DataAnnotations;

namespace RestaurantAutomation.Auth;

public sealed class AuthRequest
{
    [Required] public required string UserName { get; set; }
    [Required] public required string Password { get; set; }
}