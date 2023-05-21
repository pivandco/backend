using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityAccessControl.Auth;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly JwtService _jwt;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(JwtService jwt, UserManager<IdentityUser> userManager)
    {
        _jwt = jwt;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> CreateBearerToken(AuthRequest request)
    {
        if (!ModelState.IsValid) return BadRequest("Bad credentials");

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null) return BadRequest("Bad credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid) return BadRequest("Bad credentials");

        var response = _jwt.CreateToken(user);

        return Ok(response);
    }
}