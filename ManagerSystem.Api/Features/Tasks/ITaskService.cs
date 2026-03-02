using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Tasks;

public interface ITaskService
{
    Task<BaseResponse<TaskResponse>> CreateTaskAsync(Guid userId, CreateTaskRequest request);
    Task<BaseResponse<List<TaskResponse>>> ListTasksAsync(TaskListQuery query);
    Task<BaseResponse<TaskResponse>> UpdateTaskAsync(Guid userId, Guid id, UpdateTaskRequest request);
    Task<BaseResponse<bool>> DeleteTaskAsync(Guid id);
    Task<BaseResponse<ColumnResponse>> CreateColumnAsync(CreateColumnRequest request);
    Task<BaseResponse<ColumnResponse>> UpdateColumnAsync(Guid id, UpdateColumnRequest request);
    Task<BaseResponse<bool>> DeleteColumnAsync(Guid id);
    Task<BaseResponse<List<ColumnResponse>>> ListColumnsAsync(Guid projectId);
    Task<BaseResponse<List<TaskHistoryResponse>>> ListHistoryAsync(Guid taskId);
}
