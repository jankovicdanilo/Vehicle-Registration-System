using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RegistracijaVozila.Data;
using RegistracijaVozila.Models.DTO;
using RegistracijaVozila.Results;
using RegistracijaVozila.Services.Interface;
using System.Security.Claims;

namespace RegistracijaVozila.Services.Implementation
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

        public async Task<RepositoryResult<UserDto>> RegisterAsync(RegisterRequestDto request, ClaimsPrincipal currentUser)
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
                return RepositoryResult<UserDto>.Fail(string.Join(" | ", errors));
            }

            await userManager.AddToRoleAsync(user, "Zaposleni");

            if (request.Roles != null && request.Roles.Any())
            {
                foreach (var role in request.Roles)
                {
                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                        role.Equals("Šef odsjeka", StringComparison.OrdinalIgnoreCase) ||
                        role.Equals("SefOdsjeka", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!currentUser.IsInRole("Admin"))
                        {
                            return RepositoryResult<UserDto>.Fail("ADMIN_ONLY: Only admins can assign roles.");
                        }

                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            return RepositoryResult<UserDto>.Fail($"INVALID_ROLE: {role} role does not exist.");
                        }

                        await userManager.AddToRoleAsync(user, role);

                        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            var sefRole = "SefOdsjeka"; 
                            if (await roleManager.RoleExistsAsync(sefRole))
                            {
                                await userManager.AddToRoleAsync(user, sefRole);
                            }
                        }
                    }
                }
            }

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return RepositoryResult<UserDto>.Ok(response, "New user has successfully been created");
        }


        public async Task<RepositoryResult<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var identityUser = await userManager.FindByNameAsync(request.Username);

            if (identityUser == null)
            {
                return RepositoryResult<LoginResponseDto>.Fail($"INVALID_USERNAME: Username {request.Username} not found");
            }

            if (!string.Equals( identityUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                return RepositoryResult<LoginResponseDto>.Fail($"INVALID_EMAIL: Email {request.Email} not found");
            }

            var passwordValid = await userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!passwordValid)
            {
                return RepositoryResult<LoginResponseDto>.Fail("INVALID_PASSWORD: Incorrect password");
            }

            var token = await tokenService.GenerateJwtTokenAsync(identityUser);
            var roles = await userManager.GetRolesAsync(identityUser);

            var response = mapper.Map<LoginResponseDto>(identityUser);
            response.Token = token;
            response.Roles = roles.ToList();


            return RepositoryResult<LoginResponseDto>.Ok(response, "Login successfull");
        }

        public async Task<RepositoryResult<UserDto>> DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RepositoryResult<UserDto>.Fail($"USER_NOT_FOUND: User with id {id} not found");
            }

            if(user.Email == "danilo@gmail.com")
            {
                return RepositoryResult<UserDto>.Fail($"MAIN_ADMIN: User can't be deleted");
            }

            authDbContext.Users.Remove(user);
            await authDbContext.SaveChangesAsync();

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return RepositoryResult<UserDto>.Ok(response, "User has successfully been deleted");
        }

        public async Task<RepositoryResult<UserDto>> UpdateUserAsync
                        (UpdateUserRequestDto request, ClaimsPrincipal currentUser)
        {
            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return RepositoryResult<UserDto>.Fail($"USER_NOT_FOUND: User with id {request.Id} not found");
            }

            if (user.Email.Equals("danilo@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                user.Email = request.Email?.Trim();
                user.UserName = request.Username?.Trim();

                var resultProtected = await userManager.UpdateAsync(user);
                if (!resultProtected.Succeeded)
                {
                    return RepositoryResult<UserDto>.Fail($"UPDATE_FAILED: Really, you just did that?");
                }

                var responseProtected = mapper.Map<UserDto>(user);
                responseProtected.Roles = await userManager.GetRolesAsync(user);

                return RepositoryResult<UserDto>.Ok(responseProtected, "Protected Admin user updated (roles unchanged)");
            }

            user.Email = request.Email?.Trim();
            user.UserName = request.Username?.Trim();

            var requestedRoles = request.Roles?.Select(r => r.Trim()).Distinct().ToList() ?? new();

            foreach (var role in requestedRoles)
            {
                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !currentUser.IsInRole("Admin"))
                {
                    return RepositoryResult<UserDto>.Fail("ADMIN_ONLY: Only admins can assign the Admin role.");
                }

                if (!await roleManager.RoleExistsAsync(role))
                {
                    return RepositoryResult<UserDto>.Fail($"INVALID_ROLE: Requested role {role} does not exist.");
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
                    var sefRole = "SefOdsjeka";
                    if (await roleManager.RoleExistsAsync(sefRole) && !finalRoles.Contains(sefRole))
                    {
                        finalRoles.Add(sefRole);
                    }
                }
            }

            await userManager.AddToRoleAsync(user, "Zaposleni");
            await userManager.AddToRolesAsync(user, finalRoles);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return RepositoryResult<UserDto>.Fail($"UPDATE_FAILED: {errorMessage}");
            }

            var response = mapper.Map<UserDto>(user);
            response.Roles = finalRoles;

            return RepositoryResult<UserDto>.Ok(response, "User data has successfully been updated");
        }


        public async Task<RepositoryResult<UserDto>> ChangePasswordAsync
            (string userId, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RepositoryResult<UserDto>.Fail($"USER_NOT_FOUND: User with id {userId} not found");
            }

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return RepositoryResult<UserDto>.Fail($"CHANGE_PASSWORD_FAILED: {errorMessage}");
            }

            await userManager.UpdateSecurityStampAsync(user);

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return RepositoryResult<UserDto>.Ok(response, "Password has successfully been updated");

        }

        public async Task<RepositoryResult<UserDto>> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RepositoryResult<UserDto>.Fail($"USER_NOT_FOUND: User with id {userId} not found");
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return RepositoryResult<UserDto>.Fail($"RESET_PASSWORD_FAILED: {errorMessage}");
            }

            await userManager.UpdateSecurityStampAsync(user);

            var response = mapper.Map<UserDto>(user);
            response.Roles = await userManager.GetRolesAsync(user);

            return RepositoryResult<UserDto>.Ok(response, "Password has successfully been reset");
        }

        public async Task<RepositoryResult<IEnumerable<UserDto>>> GetAll(string callerRole)
        {
            var users = await userManager.Users.ToListAsync();
            var results = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                string mainRole;
                if (roles.Contains("Admin"))
                    mainRole = "Admin";
                else if (roles.Contains("SefOdsjeka"))
                    mainRole = "SefOdsjeka";
                else
                    mainRole = "Zaposleni";

                results.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles,     
                    MainRole = mainRole
                });
            }

            if (callerRole == "SefOdsjeka")
            {
                results = results
                    .Where(u => u.Roles.Count() == 1 && u.Roles.Contains("Zaposleni"))
                    .ToList();
            }

            return RepositoryResult<IEnumerable<UserDto>>.Ok(results);
        }


        public async Task<RepositoryResult<UserDto>> GetUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RepositoryResult<UserDto>.Fail($"USER_NOT_FOUND: User with id {id} not found");
            }

            var role = await userManager.GetRolesAsync(user);

            var response = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = role
            };
            
            return RepositoryResult<UserDto>.Ok(response);
        }
    }
}






