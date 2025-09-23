using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegistracijaVozila.Models.DTO;
using RegistracijaVozila.Services.Interface;

namespace RegistracijaVozila.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthService authService;

        public AuthController(UserManager<IdentityUser> userManager, 
            IAuthService authService)
        {
            this.userManager = userManager;
            this.authService = authService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SefOdsjeka")]
        public async Task<IActionResult> List()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(currentUser);

            string? callerRole = null;

            if (roles.Contains("Admin"))
                callerRole = "Admin";
            else if (roles.Contains("SefOdsjeka"))
                callerRole = "SefOdsjeka";

            if (callerRole == null)
                return Forbid();

            var result = await authService.GetAll(callerRole);

            return Ok(result.Data);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var result = await authService.GetUserAsync(id);

            if(!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts[1] : result.Message
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var currentUser = User;
            var result = await authService.RegisterAsync(request, currentUser);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts[1] : result.Message
                });
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await authService.LoginAsync(request);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts[1] : result.Message
                });
            }

            return Ok(new { Token = result });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await authService.DeleteAsync(id);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts[1] : result.Message
                });
            }

            return Ok(result);
        }

        [HttpPut("updateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto request)
        {
            var currentUser = User;
            var result = await authService.UpdateUserAsync(request, currentUser);

            if (!result.Success)
            {
                return BadRequest(result.Errors.Any() ? result.Errors : new[] { result.Message });
            }

            return Ok(result);
        }

        [HttpPut("changePassword")]
        [Authorize(Roles = "Admin, SefOdsjeka")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeRequestDto request)
        {
            var userId = userManager.GetUserId(User);

            var result = await authService.ChangePasswordAsync
                (userId, request.CurrentPassword, request.NewPassword);

            if (!result.Success)
            {
                return BadRequest(result.Errors.Any() ? result.Errors : new[] { result.Message });
            }

            return Ok(result);
        }

        [HttpPut("resetPassword")]
        [Authorize(Roles = "Admin, SefOdsjeka")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequestDto request)
        {
            var result = await authService.ResetPasswordAsync(request.Id, request.NewPassword);

            if (!result.Success)
            {
                return BadRequest(result.Errors.Any() ? result.Errors : new[] { result.Message });
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("vratiNesto")]
        public async Task<IActionResult> VratiNesto()
        {
            var response = new
            {
                message = "Đe si, što ima?",
                time = DateTime.Now
            };

            return Ok(response);
        }
            
    }
}
