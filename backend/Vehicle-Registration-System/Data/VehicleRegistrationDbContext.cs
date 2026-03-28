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

            modelBuilder.Entity<Registration>()
                .HasIndex(r => r.VehicleId)
                .IsUnique().
                HasDatabaseName("UQ_Registration_VehicleId");

            modelBuilder.Entity<Registration>()
                .HasIndex(r => r.LicensePlate)
                .IsUnique()
                .HasDatabaseName("UQ_Registration_LicensePlate");

            var carTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var truckTypeId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var motorcycleTypeId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType { Id = motorcycleTypeId, Name = "Motorcycle", Category = "A" },
                new VehicleType { Id = carTypeId, Name = "Car", Category = "B" },
                new VehicleType { Id = truckTypeId, Name = "Truck", Category = "C" }
            );

            var toyotaId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var yamahaId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var mercedesId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            modelBuilder.Entity<VehicleBrand>().HasData(
                new VehicleBrand { Id = yamahaId, Name = "Yamaha", VehicleTypeId = motorcycleTypeId },
                new VehicleBrand { Id = toyotaId, Name = "Toyota", VehicleTypeId = carTypeId },
                new VehicleBrand { Id = mercedesId, Name = "Mercedes", VehicleTypeId = truckTypeId }
            );

            var corollaId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var r1Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
            var actrosId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

            modelBuilder.Entity<VehicleModel>().HasData(
                new VehicleModel { Id = corollaId, Name = "Corolla", VehicleBrandId = toyotaId },
                new VehicleModel { Id = r1Id, Name = "R1", VehicleBrandId = yamahaId },
                new VehicleModel { Id = actrosId, Name = "Actros", VehicleBrandId = mercedesId }
            );

            var vehicle1 = Guid.Parse("30303030-3030-3030-3030-303030303030");
            var vehicle2 = Guid.Parse("40404040-4040-4040-4040-404040404040");
            var vehicle3 = Guid.Parse("50505050-5050-5050-5050-505050505050");

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = vehicle1,
                    VehicleTypeId = motorcycleTypeId,
                    VehicleBrandId = yamahaId,
                    VehicleModelId = r1Id,
                    ProductionYear = 2022,
                    EngineCapacity = 150,
                    Weight = 300,
                    EnginePowerKw = 35,
                    ChassisNumber = "WBAXX12345678901",
                    FirstRegistrationDate = new DateTime(2023, 3, 10),
                    FuelType = "Petrol"
                },
                new Vehicle
                {
                    Id = vehicle2,
                    VehicleTypeId = carTypeId,
                    VehicleBrandId = toyotaId,
                    VehicleModelId = corollaId,
                    ProductionYear = 2020,
                    EngineCapacity = 1800,
                    Weight = 1400,
                    EnginePowerKw = 65,
                    ChassisNumber = "JTDBR32E720123456",
                    FirstRegistrationDate = new DateTime(2022, 6, 1),
                    FuelType = "Diesel"
                },
                new Vehicle
                {
                    Id = vehicle3,
                    VehicleTypeId = truckTypeId,
                    VehicleBrandId = mercedesId,
                    VehicleModelId = actrosId,
                    ProductionYear = 2020,
                    EngineCapacity = 4000,
                    Weight = 4200,
                    EnginePowerKw = 90,
                    ChassisNumber = "ACAEBF12345678902",
                    FirstRegistrationDate = new DateTime(2023, 3, 10),
                    FuelType = "Diesel"
                }
            );

            var client1 = Guid.Parse("10101010-1010-1010-1010-101010101010");
            var client2 = Guid.Parse("20202020-2020-2020-2020-202020202020");

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = client1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "John@test.com",
                    PhoneNumber = "987654",
                    NationalId = "1234567890123",
                    IdCardNumber = "123456",
                    Address = "Main Street 12",
                    DateOfBirth = new DateTime(1990, 3, 3)
                },
                new Client
                {
                    Id = client2,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "Alice@test.com",
                    PhoneNumber = "123789",
                    NationalId = "9876543210123",
                    IdCardNumber = "987321",
                    Address = "Second Street 5",
                    DateOfBirth = new DateTime(2002, 4, 4)
                }
            );

            var allianzId = Guid.Parse("50505050-5050-5050-5050-505050505050");
            var generaliId = Guid.Parse("60606060-6060-6060-6060-606060606060");

            modelBuilder.Entity<Insurance>().HasData(
                new Insurance { Id = allianzId, Name = "Allianz" },
                new Insurance { Id = generaliId, Name = "Generali" }
            );

            modelBuilder.Entity<InsurancePrice>().HasData(
                new InsurancePrice
                {
                    Id = Guid.Parse("70707070-7070-7070-7070-707070707070"),
                    InsuranceId = allianzId,
                    MinKw = 1,
                    MaxKw = 50,
                    PricePerKw = 220
                },
                new InsurancePrice
                {
                    Id = Guid.Parse("80808080-8080-8080-8080-808080808080"),
                    InsuranceId = allianzId,
                    MinKw = 51,
                    MaxKw = 100,
                    PricePerKw = 270
                },
                new InsurancePrice
                {
                    Id = Guid.Parse("156f70c9-94bf-40e6-ad5b-8efa662f3a22"),
                    InsuranceId = generaliId,
                    MinKw = 1,
                    MaxKw = 50,
                    PricePerKw = 225
                },
                new InsurancePrice
                {
                    Id = Guid.Parse("8dade321-ff73-43f0-9cf0-2c3ec1ce0700"),
                    InsuranceId = generaliId,
                    MinKw = 51,
                    MaxKw = 100,
                    PricePerKw = 265
                }
            );

            modelBuilder.Entity<Registration>().HasData(
                new Registration
                {
                    Id = Guid.Parse("90909090-9090-9090-9090-909090909090"),
                    RegistrationDate = new DateTime(2025, 1, 1),
                    ExpirationDate = new DateTime(2026, 1, 1),
                    RegistrationPrice = 55000,
                    LicensePlate = "PG123AB",
                    IsTemporary = false,
                    ClientId = client1,
                    VehicleId = vehicle1,
                    InsuranceId = allianzId
                }
            );
        }
    }
}