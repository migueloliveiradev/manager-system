using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Users;

public interface IUserService
{
    Task<BaseResponse<UserResponse>> GetAsync(Guid id);
    Task<BaseResponse<List<UserResponse>>> ListAsync();
    Task<BaseResponse<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<BaseResponse<bool>> DeactivateAsync(Guid id);
    Task<BaseResponse<bool>> ChangePasswordAsync(Guid id, ChangePasswordRequest request);
}
