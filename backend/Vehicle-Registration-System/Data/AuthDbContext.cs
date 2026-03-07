using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace VehicleRegistrationSystem.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "80e38d55-3872-4320-9f5b-54d5b2e50d38";
            var employeeRoleId = "49b719cf-9ebf-4151-af09-835f9c03f6b6";
            var managerRoleId = "f2f2ca54-7b0e-4d50-9b8a-7a5f9d6e3a11";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = managerRoleId
                },
                new IdentityRole
                {
                    Id = employeeRoleId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = employeeRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "93e88597-7567-4e0a-af32-d3c79597ab97";
            var managerUserId = "704519ee-9221-42a4-bc6e-b09e9b7dc72e";
            var employeeUserId = "5debe77c-d7dc-4321-b09d-e8e3fe2d92eb";
            var hasher = new PasswordHasher<IdentityUser>();


            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "admin@test.com",
                NormalizedEmail = "ADMIN@TEST.COM",
                NormalizedUserName = "ADMIN",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var manager = new IdentityUser
            {
                Id = managerUserId,
                UserName = "manager",
                Email = "manager@test.com",
                NormalizedEmail = "MANAGER@TEST.COM",
                NormalizedUserName = "MANAGER",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            manager.PasswordHash = hasher.HashPassword(manager, "Manager123!");

            var employee = new IdentityUser
            {
                Id = employeeUserId,
                UserName = "employee",
                Email = "employee@test.com",
                NormalizedEmail = "EMPLOYEE@TEST.COM",
                NormalizedUserName = "EMPLOYEE",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            employee.PasswordHash = hasher.HashPassword(employee, "Employee123!");

            builder.Entity<IdentityUser>().HasData(admin, manager, employee);

            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = employeeRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = managerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}