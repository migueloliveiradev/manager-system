namespace ManagerSystem.Api.Infrastructure;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "ManagerSystem";
    public string Audience { get; set; } = "ManagerSystem.App";
    public string Key { get; set; } = "change-me-super-long-jwt-key";
    public int AccessTokenMinutes { get; set; } = 30;
    public int RefreshTokenDays { get; set; } = 7;
}
