namespace ManagerSystem.Api.Features.Auth;

public record RegisterRequest(string FullName, string Email, string Password, string Role);
public record LoginRequest(string Email, string Password);
public record RefreshRequest(string RefreshToken);
public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
