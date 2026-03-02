using ManagerSystem.Api.Common;
using ManagerSystem.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Api.Features.Users;

public class UserService(UserManager<AppUser> userManager) : IUserService
{
    public async Task<BaseResponse<UserResponse>> GetAsync(Guid id)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        return user is null ? BaseResponse<UserResponse>.Failure("User not found.") : BaseResponse<UserResponse>.Success(ToDto(user));
    }

    public Task<BaseResponse<List<UserResponse>>> ListAsync() => Task.FromResult(BaseResponse<List<UserResponse>>.Success(userManager.Users.Select(ToDto).ToList()));

    public async Task<BaseResponse<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return BaseResponse<UserResponse>.Failure("User not found.");
        user.FullName = request.FullName;
        user.ProfilePhotoUrl = request.ProfilePhotoUrl;
        user.IsActive = request.IsActive;
        await userManager.UpdateAsync(user);
        return BaseResponse<UserResponse>.Success(ToDto(user));
    }

    public async Task<BaseResponse<bool>> DeactivateAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return BaseResponse<bool>.Failure("User not found.");
        user.IsActive = false;
        await userManager.UpdateAsync(user);
        return BaseResponse<bool>.Success(true);
    }

    public async Task<BaseResponse<bool>> ChangePasswordAsync(Guid id, ChangePasswordRequest request)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return BaseResponse<bool>.Failure("User not found.");
        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.Failure(result.Errors.Select(x => x.Description).ToArray());
    }

    private static UserResponse ToDto(AppUser user) => new(user.Id, user.FullName, user.Email ?? string.Empty, user.ProfilePhotoUrl, user.IsActive);
}
