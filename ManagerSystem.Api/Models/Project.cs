using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;
    public Guid CreatedById { get; set; }
    public AppUser CreatedBy { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public List<TaskColumn> Columns { get; set; } = [];
}
