namespace ManagerSystem.Api.Models;

public class TaskHistory
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public WorkTask Task { get; set; } = default!;
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
