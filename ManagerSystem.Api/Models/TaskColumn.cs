namespace ManagerSystem.Api.Models;

public class TaskColumn
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}
