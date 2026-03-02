using ManagerSystem.Api.Features.Auth;

namespace ManagerSystem.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");
        group.MapPost("/register", (RegisterRequest request, IAuthService service) => service.RegisterAsync(request));
        group.MapPost("/login", (LoginRequest request, IAuthService service) => service.LoginAsync(request));
        group.MapPost("/refresh", (RefreshRequest request, IAuthService service) => service.RefreshAsync(request));
        return group;
    }
}
