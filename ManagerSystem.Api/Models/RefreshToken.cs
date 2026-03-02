namespace ManagerSystem.Api.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public bool Revoked { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;
}
