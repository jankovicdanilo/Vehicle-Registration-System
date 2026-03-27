using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using System.Security.Claims;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.DTO.Auth;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext authDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(AuthDbContext authDbContext, UserManager<IdentityUser> userManager,
            ITokenService tokenService, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            this.authDbContext = authDbContext;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        public async Task<Result<UserDto>> RegisterAsync
            (RegisterRequestDto request, ClaimsPrincipal currentUser)
        {
            var user = new IdentityUser
            {
                UserName = request.Username?.Trim(),
                Email = request.Email?.Trim()
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return Result<UserDto>.Fail(errors);
            }

            await userManager.AddToRoleAsync(user, "Employee");

            if (request.Roles != null && request.Roles.Any())
            {
                foreach (var role in request.Roles)
                {
                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                        role.Equals("Manager", StringComparison.OrdinalIgnoreCase) ||
                        role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!currentUser.IsInRole("Admin"))
                        {
                            return Result<UserDto>.Fail("ADMIN_ONLY", "Only admins can assign roles.");
                        }

                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            return Result<UserDto>.Fail($"INVALID_ROLE", $"{role} role does not exist.");
                        }

                        await userManager.AddToRoleAsync(user, role);

                        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            var managerRole = "Manager"; 
                            if (await roleManager.RoleExistsAsync(managerRole))
                            {
                                await userManager.AddToRoleAsync(user, managerRole);
                            }
                        }
                    }
                }
            }

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return Result<UserDto>.Ok(response, "New user has successfully been created");
        }


        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var identityUser = await userManager.FindByNameAsync(request.Username);

            if (identityUser == null)
            {
                return Result<LoginResponseDto>.Fail
                    ($"INVALID_USERNAME", "Username {request.Username} not found");
            }

            if (!string.Equals( identityUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                return Result<LoginResponseDto>.Fail($"INVALID_EMAIL", "Email {request.Email} not found");
            }

            var passwordValid = await userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!passwordValid)
            {
                return Result<LoginResponseDto>.Fail("INVALID_PASSWORD", "Incorrect password");
            }

            var token = await tokenService.GenerateJwtTokenAsync(identityUser);
            var roles = await userManager.GetRolesAsync(identityUser);

            var response = mapper.Map<LoginResponseDto>(identityUser);
            response.Token = token;
            response.Roles = roles.ToList();


            return Result<LoginResponseDto>.Ok(response, "Login successfull");
        }

        public async Task<Result<UserDto>> DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Result<UserDto>.Fail($"USER_NOT_FOUND", "User with id {id} not found");
            }

            if(user.Email == "danilo@gmail.com")
            {
                return Result<UserDto>.Fail($"MAIN_ADMIN", "User can't be deleted");
            }

            authDbContext.Users.Remove(user);
            await authDbContext.SaveChangesAsync();

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return Result<UserDto>.Ok(response, "User has successfully been deleted");
        }

        public async Task<Result<UserDto>> UpdateUserAsync
                        (UpdateUserRequestDto request, ClaimsPrincipal currentUser)
        {
            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return Result<UserDto>.Fail($"USER_NOT_FOUND", "User with id {request.Id} not found");
            }

            if (user.Email.Equals("danilo@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                user.Email = request.Email?.Trim();
                user.UserName = request.Username?.Trim();

                var resultProtected = await userManager.UpdateAsync(user);
                if (!resultProtected.Succeeded)
                {
                    return Result<UserDto>.Fail($"UPDATE_FAILED", "Really, you just did that?");
                }

                var responseProtected = mapper.Map<UserDto>(user);
                responseProtected.Roles = await userManager.GetRolesAsync(user);

                return Result<UserDto>.Ok(responseProtected, 
                    "Protected Admin user updated (roles unchanged)");
            }

            user.Email = request.Email?.Trim();
            user.UserName = request.Username?.Trim();

            var requestedRoles = request.Roles?.Select(r => r.Trim()).Distinct().ToList() ?? new();

            foreach (var role in requestedRoles)
            {
                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !currentUser.IsInRole("Admin"))
                {
                    return Result<UserDto>.Fail("ADMIN_ONLY", "Only admins can assign the Admin role.");
                }

                if (!await roleManager.RoleExistsAsync(role))
                {
                    return Result<UserDto>.Fail($"INVALID_ROLE" , "Requested role {role} does not exist.");
                }
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);

            var finalRoles = new List<string>();

            foreach (var role in requestedRoles)
            {
                finalRoles.Add(role);

                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    var sefRole = "Manager";
                    if (await roleManager.RoleExistsAsync(sefRole) && !finalRoles.Contains(sefRole))
                    {
                        finalRoles.Add(sefRole);
                    }
                }
            }

            await userManager.AddToRoleAsync(user, "Employee");
            await userManager.AddToRolesAsync(user, finalRoles);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result<UserDto>.Fail("UPDATE_FAILED", errorMessage);
            }

            var response = mapper.Map<UserDto>(user);
            response.Roles = finalRoles;

            return Result<UserDto>.Ok(response, "User data has successfully been updated");
        }


        public async Task<Result<UserDto>> ChangePasswordAsync
            (string userId, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result<UserDto>.Fail($"USER_NOT_FOUND" ,"User with id {userId} not found");
            }

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result<UserDto>.Fail($"CHANGE_PASSWORD_FAILED", errorMessage);
            }

            await userManager.UpdateSecurityStampAsync(user);

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return Result<UserDto>.Ok(response, "Password has successfully been updated");

        }

        public async Task<Result<UserDto>> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result<UserDto>.Fail($"USER_NOT_FOUND", "User with id {userId} not found");
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result<UserDto>.Fail($"RESET_PASSWORD_FAILED", errorMessage);
            }

            await userManager.UpdateSecurityStampAsync(user);

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return Result<UserDto>.Ok(response, "Password has successfully been reset");
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAll(string callerRole)
        {
            var users = await userManager.Users.ToListAsync();
            var results = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                string mainRole;
                if (roles.Contains("Admin"))
                    mainRole = "Admin";
                else if (roles.Contains("Manager"))
                    mainRole = "Manager";
                else
                    mainRole = "Employee";

                results.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles,     
                    MainRole = mainRole
                });
            }

            if (callerRole == "Manager")
            {
                results = results
                    .Where(u => u.Roles.Count() == 1 && u.Roles.Contains("Employee"))
                    .ToList();
            }

            return Result<IEnumerable<UserDto>>.Ok(results);
        }


        public async Task<Result<UserDto>> GetUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Result<UserDto>.Fail($"USER_NOT_FOUND", "User with id {id} not found");
            }

            var role = await userManager.GetRolesAsync(user);

            var response = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = role
            };
            
            return Result<UserDto>.Ok(response);
        }
    }
}






