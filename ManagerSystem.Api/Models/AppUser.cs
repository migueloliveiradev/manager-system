using Microsoft.AspNetCore.Identity;

namespace ManagerSystem.Api.Models;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePhotoUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
