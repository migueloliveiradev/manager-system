namespace ManagerSystem.Api.Features.Users;

public record UserResponse(Guid Id, string FullName, string Email, string? ProfilePhotoUrl, bool IsActive);
public record UpdateUserRequest(string FullName, string? ProfilePhotoUrl, bool IsActive);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
