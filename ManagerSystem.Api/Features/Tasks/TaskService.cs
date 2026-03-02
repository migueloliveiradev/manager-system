using ManagerSystem.Api.Common;
using ManagerSystem.Api.Data;
using ManagerSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Api.Features.Tasks;

public class TaskService(AppDbContext db) : ITaskService
{
    public async Task<BaseResponse<TaskResponse>> CreateTaskAsync(Guid userId, CreateTaskRequest request)
    {
        var firstColumn = await db.TaskColumns.OrderBy(x => x.Order).FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId);
        if (firstColumn is null) return BaseResponse<TaskResponse>.Failure("Project has no columns.");
        var task = new WorkTask { Title = request.Title, Description = request.Description, Priority = request.Priority, DueDate = request.DueDate, ProjectId = request.ProjectId, AssigneeId = request.AssigneeId, StatusId = firstColumn.Id };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        await SaveHistoryAsync(task.Id, userId, "Task created");
        return BaseResponse<TaskResponse>.Success(ToDto(task));
    }

    public async Task<BaseResponse<List<TaskResponse>>> ListTasksAsync(Guid projectId) => BaseResponse<List<TaskResponse>>.Success(await db.Tasks.Where(x => x.ProjectId == projectId).Select(x => new TaskResponse(x.Id, x.Title, x.Description, x.Priority, x.StatusId, x.DueDate, x.ProjectId, x.AssigneeId, x.Progress)).ToListAsync());

    public async Task<BaseResponse<TaskResponse>> UpdateTaskAsync(Guid userId, Guid id, UpdateTaskRequest request)
    {
        var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task is null) return BaseResponse<TaskResponse>.Failure("Task not found.");
        task.Title = request.Title;
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.StatusId = request.StatusId;
        task.AssigneeId = request.AssigneeId;
        task.Progress = Math.Clamp(request.Progress, 0, 100);
        await db.SaveChangesAsync();
        await SaveHistoryAsync(task.Id, userId, "Task updated");
        return BaseResponse<TaskResponse>.Success(ToDto(task));
    }

    public async Task<BaseResponse<bool>> DeleteTaskAsync(Guid userId, Guid id)
    {
        _ = userId;
        var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task is null) return BaseResponse<bool>.Failure("Task not found.");
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return BaseResponse<bool>.Success(true);
    }

    public async Task<BaseResponse<ColumnResponse>> CreateColumnAsync(CreateColumnRequest request)
    {
        var column = new TaskColumn { ProjectId = request.ProjectId, Name = request.Name, Order = request.Order };
        db.TaskColumns.Add(column);
        await db.SaveChangesAsync();
        return BaseResponse<ColumnResponse>.Success(new ColumnResponse(column.Id, column.ProjectId, column.Name, column.Order));
    }

    public async Task<BaseResponse<List<ColumnResponse>>> ListColumnsAsync(Guid projectId) => BaseResponse<List<ColumnResponse>>.Success(await db.TaskColumns.Where(x => x.ProjectId == projectId).OrderBy(x => x.Order).Select(x => new ColumnResponse(x.Id, x.ProjectId, x.Name, x.Order)).ToListAsync());

    public async Task<BaseResponse<List<TaskHistoryResponse>>> ListHistoryAsync(Guid taskId) => BaseResponse<List<TaskHistoryResponse>>.Success(await db.TaskHistories.Where(x => x.TaskId == taskId).OrderByDescending(x => x.CreatedAtUtc).Select(x => new TaskHistoryResponse(x.Id, x.TaskId, x.UserId, x.Action, x.CreatedAtUtc)).ToListAsync());

    private async Task SaveHistoryAsync(Guid taskId, Guid userId, string action)
    {
        db.TaskHistories.Add(new TaskHistory { TaskId = taskId, UserId = userId, Action = action });
        await db.SaveChangesAsync();
    }

    private static TaskResponse ToDto(WorkTask task) => new(task.Id, task.Title, task.Description, task.Priority, task.StatusId, task.DueDate, task.ProjectId, task.AssigneeId, task.Progress);
}
