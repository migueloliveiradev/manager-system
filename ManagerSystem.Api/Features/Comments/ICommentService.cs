using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Comments;

public interface ICommentService
{
    Task<BaseResponse<CommentResponse>> CreateAsync(Guid userId, CreateCommentRequest request);
    Task<BaseResponse<List<CommentResponse>>> ListAsync(Guid taskId);
    Task<BaseResponse<CommentResponse>> UpdateAsync(Guid userId, Guid id, UpdateCommentRequest request);
    Task<BaseResponse<bool>> DeleteAsync(Guid userId, Guid id);
}
