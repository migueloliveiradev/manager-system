using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Tasks;

public interface ITaskService
{
    Task<BaseResponse<TaskResponse>> CreateTaskAsync(Guid userId, CreateTaskRequest request);
    Task<BaseResponse<List<TaskResponse>>> ListTasksAsync(Guid projectId);
    Task<BaseResponse<TaskResponse>> UpdateTaskAsync(Guid userId, Guid id, UpdateTaskRequest request);
    Task<BaseResponse<bool>> DeleteTaskAsync(Guid userId, Guid id);
    Task<BaseResponse<ColumnResponse>> CreateColumnAsync(CreateColumnRequest request);
    Task<BaseResponse<List<ColumnResponse>>> ListColumnsAsync(Guid projectId);
    Task<BaseResponse<List<TaskHistoryResponse>>> ListHistoryAsync(Guid taskId);
}
