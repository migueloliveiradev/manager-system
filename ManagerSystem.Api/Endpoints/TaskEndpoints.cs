using ManagerSystem.Api.Features.Tasks;
using ManagerSystem.Api.Infrastructure;

namespace ManagerSystem.Api.Endpoints;

public static class TaskEndpoints
{
    public static RouteGroupBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").RequireAuthorization();
        group.MapPost("/", (HttpContext ctx, CreateTaskRequest request, ITaskService service) => service.CreateTaskAsync(ctx.GetUserId(), request));
        group.MapGet("/project/{projectId:guid}", (Guid projectId, ITaskService service) => service.ListTasksAsync(projectId));
        group.MapPut("/{id:guid}", (HttpContext ctx, Guid id, UpdateTaskRequest request, ITaskService service) => service.UpdateTaskAsync(ctx.GetUserId(), id, request));
        group.MapDelete("/{id:guid}", (HttpContext ctx, Guid id, ITaskService service) => service.DeleteTaskAsync(ctx.GetUserId(), id));
        group.MapGet("/{taskId:guid}/history", (Guid taskId, ITaskService service) => service.ListHistoryAsync(taskId));

        var columns = app.MapGroup("/api/columns").RequireAuthorization();
        columns.MapPost("/", (CreateColumnRequest request, ITaskService service) => service.CreateColumnAsync(request));
        columns.MapGet("/{projectId:guid}", (Guid projectId, ITaskService service) => service.ListColumnsAsync(projectId));
        return group;
    }
}
