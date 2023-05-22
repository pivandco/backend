using Microsoft.AspNetCore.Identity;

namespace UniversityAccessControl;

public sealed class UserInitializer
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserInitializer(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task InitializeAsync()
    {
        if (await _userManager.FindByNameAsync("admin") == null)
        {
            await _userManager.CreateAsync(new IdentityUser("admin"), "Admin123!");
        }
    }
}