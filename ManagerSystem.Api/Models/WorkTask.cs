using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Models;

public class WorkTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public Guid StatusId { get; set; }
    public TaskColumn Status { get; set; } = default!;
    public DateTime? DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public Guid? AssigneeId { get; set; }
    public AppUser? Assignee { get; set; }
    public int Progress { get; set; }
}
