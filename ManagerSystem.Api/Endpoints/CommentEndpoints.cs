using ManagerSystem.Api.Features.Comments;
using ManagerSystem.Api.Infrastructure;

namespace ManagerSystem.Api.Endpoints;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/comments").RequireAuthorization();
        group.MapPost("/", (HttpContext ctx, CreateCommentRequest request, ICommentService service) => service.CreateAsync(ctx.GetUserId(), request));
        group.MapGet("/{taskId:guid}", (Guid taskId, ICommentService service) => service.ListAsync(taskId));
        group.MapPut("/{id:guid}", (HttpContext ctx, Guid id, UpdateCommentRequest request, ICommentService service) => service.UpdateAsync(ctx.GetUserId(), id, request));
        group.MapDelete("/{id:guid}", (HttpContext ctx, Guid id, ICommentService service) => service.DeleteAsync(ctx.GetUserId(), id));
        return group;
    }
}
