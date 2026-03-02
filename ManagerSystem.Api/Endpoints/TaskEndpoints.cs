using ManagerSystem.Api.Features.Tasks;
using ManagerSystem.Api.Infrastructure;
using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Endpoints;

public static class TaskEndpoints
{
    public static RouteGroupBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").RequireAuthorization();
        group.MapPost("/", (HttpContext ctx, CreateTaskRequest request, ITaskService service) => service.CreateTaskAsync(ctx.GetUserId(), request));
        group.MapGet("/", (Guid? projectId, Guid? assigneeId, TaskPriority? priority, Guid? statusId, DateTime? dueDateUntil, ITaskService service) => service.ListTasksAsync(new TaskListQuery(projectId, assigneeId, priority, statusId, dueDateUntil)));
        group.MapPut("/{id:guid}", (HttpContext ctx, Guid id, UpdateTaskRequest request, ITaskService service) => service.UpdateTaskAsync(ctx.GetUserId(), id, request));
        group.MapDelete("/{id:guid}", (Guid id, ITaskService service) => service.DeleteTaskAsync(id));
        group.MapGet("/{taskId:guid}/history", (Guid taskId, ITaskService service) => service.ListHistoryAsync(taskId));

        var columns = app.MapGroup("/api/columns").RequireAuthorization();
        columns.MapPost("/", (CreateColumnRequest request, ITaskService service) => service.CreateColumnAsync(request));
        columns.MapPut("/{id:guid}", (Guid id, UpdateColumnRequest request, ITaskService service) => service.UpdateColumnAsync(id, request));
        columns.MapDelete("/{id:guid}", (Guid id, ITaskService service) => service.DeleteColumnAsync(id));
        columns.MapGet("/{projectId:guid}", (Guid projectId, ITaskService service) => service.ListColumnsAsync(projectId));
        return group;
    }
}
