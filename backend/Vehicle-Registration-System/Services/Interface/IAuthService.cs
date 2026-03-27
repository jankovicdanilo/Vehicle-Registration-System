using VehicleRegistrationSystem.Results;
using System.Security.Claims;
using VehicleRegistrationSystem.Models.DTO.Auth;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IAuthService
    {
        Task<Result<IEnumerable<UserDto>>> GetAll(string callerRole);

        Task<Result<UserDto>> RegisterAsync(RegisterRequestDto request, ClaimsPrincipal currentUser);

        Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);

        Task<Result<UserDto>> DeleteAsync(string id);

        Task<Result<UserDto>> UpdateUserAsync
            (UpdateUserRequestDto request, ClaimsPrincipal currentUser);

        Task<Result<UserDto>> ChangePasswordAsync
            (string userId, string currentPassword, string newPassword);

        Task<Result<UserDto>> ResetPasswordAsync(string userId, string newPassword);

        Task<Result<UserDto>> GetUserAsync(string id);
    }
}
