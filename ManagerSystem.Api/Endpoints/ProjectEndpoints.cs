using ManagerSystem.Api.Features.Projects;
using ManagerSystem.Api.Infrastructure;
using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").RequireAuthorization();
        group.MapPost("/", (HttpContext ctx, CreateProjectRequest request, IProjectService service) => service.CreateAsync(ctx.GetUserId(), request));
        group.MapGet("/", (ProjectStatus? status, string? search, IProjectService service) => service.ListAsync(new ProjectListQuery(status, search)));
        group.MapGet("/{id:guid}", (Guid id, IProjectService service) => service.GetAsync(id));
        group.MapPut("/{id:guid}", (Guid id, UpdateProjectRequest request, IProjectService service) => service.UpdateAsync(id, request));
        group.MapDelete("/{id:guid}", (Guid id, IProjectService service) => service.DeleteAsync(id));
        return group;
    }
}
