using ManagerSystem.Api.Common;
using ManagerSystem.Api.Data;
using ManagerSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Api.Features.Comments;

public class CommentService(AppDbContext db) : ICommentService
{
    public async Task<BaseResponse<CommentResponse>> CreateAsync(Guid userId, CreateCommentRequest request)
    {
        var comment = new Comment { UserId = userId, TaskId = request.TaskId, Content = request.Content };
        db.Comments.Add(comment);
        await db.SaveChangesAsync();
        return BaseResponse<CommentResponse>.Success(ToDto(comment));
    }

    public async Task<BaseResponse<List<CommentResponse>>> ListAsync(Guid taskId) => BaseResponse<List<CommentResponse>>.Success(await db.Comments.Where(x => x.TaskId == taskId).OrderBy(x => x.CreatedAtUtc).Select(x => new CommentResponse(x.Id, x.TaskId, x.UserId, x.Content, x.CreatedAtUtc)).ToListAsync());

    public async Task<BaseResponse<CommentResponse>> UpdateAsync(Guid userId, Guid id, UpdateCommentRequest request)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (comment is null) return BaseResponse<CommentResponse>.Failure("Comment not found.");
        comment.Content = request.Content;
        await db.SaveChangesAsync();
        return BaseResponse<CommentResponse>.Success(ToDto(comment));
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid userId, Guid id)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (comment is null) return BaseResponse<bool>.Failure("Comment not found.");
        db.Comments.Remove(comment);
        await db.SaveChangesAsync();
        return BaseResponse<bool>.Success(true);
    }

    private static CommentResponse ToDto(Comment comment) => new(comment.Id, comment.TaskId, comment.UserId, comment.Content, comment.CreatedAtUtc);
}
