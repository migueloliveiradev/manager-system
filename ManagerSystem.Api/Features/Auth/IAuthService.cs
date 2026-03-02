using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Auth;

public interface IAuthService
{
    Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<BaseResponse<AuthResponse>> RefreshAsync(RefreshRequest request);
}
