using System.Security.Claims;

namespace ManagerSystem.Api.Infrastructure;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var raw = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.User.FindFirstValue(ClaimTypes.Name) ?? context.User.FindFirstValue("sub");
        return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
    }
}
