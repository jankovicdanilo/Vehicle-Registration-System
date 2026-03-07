using Microsoft.AspNetCore.Identity;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(IdentityUser user);
    }
}
