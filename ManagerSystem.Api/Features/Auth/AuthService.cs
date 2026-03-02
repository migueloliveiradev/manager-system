using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ManagerSystem.Api.Common;
using ManagerSystem.Api.Data;
using ManagerSystem.Api.Models;
using ManagerSystem.Api.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ManagerSystem.Api.Features.Auth;

public class AuthService(UserManager<AppUser> userManager, AppDbContext db, IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public async Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var user = new AppUser { UserName = request.Email, Email = request.Email, FullName = request.FullName };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BaseResponse<AuthResponse>.Failure(result.Errors.Select(x => x.Description).ToArray());
        await userManager.AddToRoleAsync(user, request.Role);
        return BaseResponse<AuthResponse>.Success(await BuildAuthResponseAsync(user));
    }

    public async Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive || !await userManager.CheckPasswordAsync(user, request.Password))
            return BaseResponse<AuthResponse>.Failure("Invalid credentials.");
        return BaseResponse<AuthResponse>.Success(await BuildAuthResponseAsync(user));
    }

    public async Task<BaseResponse<AuthResponse>> RefreshAsync(RefreshRequest request)
    {
        var token = await db.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
        if (token is null || token.Revoked || token.ExpiresAtUtc <= DateTime.UtcNow) return BaseResponse<AuthResponse>.Failure("Invalid refresh token.");
        token.Revoked = true;
        await db.SaveChangesAsync();
        return BaseResponse<AuthResponse>.Success(await BuildAuthResponseAsync(token.User));
    }

    private async Task<AuthResponse> BuildAuthResponseAsync(AppUser user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);
        var claims = await BuildClaimsAsync(user);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims, expires: expiresAt, signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
        var refreshToken = await StoreRefreshTokenAsync(user.Id);
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), refreshToken, expiresAt);
    }

    private async Task<List<Claim>> BuildClaimsAsync(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty), new(ClaimTypes.Name, user.FullName) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private async Task<string> StoreRefreshTokenAsync(Guid userId)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        db.RefreshTokens.Add(new RefreshToken { UserId = userId, Token = refreshToken, ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays) });
        await db.SaveChangesAsync();
        return refreshToken;
    }
}
