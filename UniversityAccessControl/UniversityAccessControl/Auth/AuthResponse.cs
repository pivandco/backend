namespace UniversityAccessControl.Auth;

public sealed class AuthResponse
{
    public required string Token { get; set; }
    public DateTime Expiration { get; set; }
}