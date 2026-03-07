using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Data
{
    public class VehicleRegistrationDbContext : DbContext
    {
        public VehicleRegistrationDbContext(DbContextOptions<VehicleRegistrationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Insurance> Insurances { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        public DbSet<VehicleBrand> VehicleBrands { get; set; }

        public DbSet<VehicleModel> VehicleModels { get; set; }

        public DbSet<InsurancePrice> InsurancePrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleModel)
                .WithMany()
                .HasForeignKey(v => v.VehicleModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleBrand)
                .WithMany()
                .HasForeignKey(v => v.VehicleBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany()
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleBrand>()
                .HasOne(b => b.VehicleType)
                .WithMany(t => t.Brands)
                .HasForeignKey(b => b.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleModel>()
                .HasOne(m => m.VehicleBrand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.VehicleBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsurancePrice>()
                .Property(p => p.PricePerKw)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Registration>()
                .Property(r => r.RegistrationPrice)
                .HasPrecision(18, 2);
        }
    }
}