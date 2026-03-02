namespace ManagerSystem.Api.Features.Comments;

public record CreateCommentRequest(Guid TaskId, string Content);
public record UpdateCommentRequest(string Content);
public record CommentResponse(Guid Id, Guid TaskId, Guid UserId, string Content, DateTime CreatedAtUtc);
