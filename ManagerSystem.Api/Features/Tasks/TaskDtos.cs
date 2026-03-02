using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Features.Tasks;

public record CreateTaskRequest(string Title, string Description, TaskPriority Priority, DateTime? DueDate, Guid ProjectId, Guid? AssigneeId);
public record UpdateTaskRequest(string Title, string Description, TaskPriority Priority, DateTime? DueDate, Guid StatusId, Guid? AssigneeId, int Progress);
public record TaskResponse(Guid Id, string Title, string Description, TaskPriority Priority, Guid StatusId, DateTime? DueDate, Guid ProjectId, Guid? AssigneeId, int Progress);
public record CreateColumnRequest(Guid ProjectId, string Name, int Order);
public record ColumnResponse(Guid Id, Guid ProjectId, string Name, int Order);
public record TaskHistoryResponse(Guid Id, Guid TaskId, Guid UserId, string Action, DateTime CreatedAtUtc);
