using ManagerSystem.Api.Features.Users;

namespace ManagerSystem.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").RequireAuthorization();
        group.MapGet("/", (IUserService service) => service.ListAsync());
        group.MapGet("/{id:guid}", (Guid id, IUserService service) => service.GetAsync(id));
        group.MapPut("/{id:guid}", (Guid id, UpdateUserRequest request, IUserService service) => service.UpdateAsync(id, request));
        group.MapPut("/{id:guid}/password", (Guid id, ChangePasswordRequest request, IUserService service) => service.ChangePasswordAsync(id, request));
        group.MapDelete("/{id:guid}", (Guid id, IUserService service) => service.DeactivateAsync(id));
        return group;
    }
}
