using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;
using VehicleRegistrationSystem.Models.DTO.Vehicle;
using Microsoft.Data.SqlClient;
using Dapper;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
    {
        private readonly string? connectionString;

        public VehicleRepository(VehicleRegistrationDbContext appDbContext,
            IConfiguration configuration) : base(appDbContext) 
        {
            connectionString = configuration.GetConnectionString("VehicleRegistrationDbConnectionString");
        }

        public async Task<(List<VehicleListItemDto> Items, int TotalCount)> GetAllAsync(
        string? searchQuery = null,
        int pageNumber = 1,
        int pageSize = 10)
        {
            using var connection = new SqlConnection(connectionString);

            var offset = (pageNumber - 1) * pageSize;

            var sql = @"
                WITH Filtered AS (
                    SELECT 
                        v.Id, 
                        v.ProductionYear, 
                        v.EngineCapacity,
                        v.EnginePowerKw,
                        v.ChassisNumber, 
                        v.FuelType,
                        vt.Name AS VehicleTypeName,
                        vb.Name AS VehicleBrandName, 
                        vm.Name AS VehicleModelName
                    FROM Vehicles v 
                    JOIN VehicleTypes vt ON v.VehicleTypeId = vt.Id
                    JOIN VehicleBrands vb ON vb.Id = v.VehicleBrandId
                    JOIN VehicleModels vm ON vm.Id = v.VehicleModelId
                    WHERE (@search IS NULL OR
                           vt.Name LIKE '%' + @search + '%' OR
                           vb.Name LIKE '%' + @search + '%' OR
                           vm.Name LIKE '%' + @search + '%')
                )

                SELECT 
                    Id,
                    ProductionYear,
                    EngineCapacity,
                    EnginePowerKw,
                    ChassisNumber,
                    FuelType,
                    VehicleTypeName,
                    VehicleBrandName,
                    VehicleModelName
                FROM Filtered
                ORDER BY Id
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

                WITH Filtered AS (
                    SELECT v.Id
                    FROM Vehicles v 
                    JOIN VehicleTypes vt ON v.VehicleTypeId = vt.Id
                    JOIN VehicleBrands vb ON vb.Id = v.VehicleBrandId
                    JOIN VehicleModels vm ON vm.Id = v.VehicleModelId
                    WHERE (@search IS NULL OR
                           vt.Name LIKE '%' + @search + '%' OR
                           vb.Name LIKE '%' + @search + '%' OR
                           vm.Name LIKE '%' + @search + '%')
                )

                SELECT COUNT(*) FROM Filtered;
            ";

            using var multi = await connection.QueryMultipleAsync(sql, new
            {
                search = searchQuery,
                offset,
                pageSize
            });

            var items = (await multi.ReadAsync<VehicleListItemDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (items, totalCount);
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(Guid id)
        {
            return await appDbContext.Vehicles
                .Where(x => x.Id == id)
                .Select(x => new VehicleDto
                {
                    Id = x.Id,
                    VehicleBrandId = x.VehicleBrandId,
                    VehicleModelId = x.VehicleModelId,
                    VehicleTypeId = x.VehicleTypeId,
                    VehicleBrandName = x.VehicleBrand.Name,
                    VehicleModelName = x.VehicleModel.Name,
                    VehicleTypeName = x.VehicleType.Name,
                    ProductionYear = x.ProductionYear,
                    EngineCapacity = x.EngineCapacity,
                    FuelType = x.FuelType,
                    Weight = x.Weight,
                    EnginePowerKw = x.EnginePowerKw,
                    ChassisNumber = x.ChassisNumber,
                    FirstRegistrationDate = x.FirstRegistrationDate,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> IsVehicleModelValidAsync(Guid modelId, Guid brandId, Guid typeId)
        {
            return await appDbContext.VehicleModels.Include(m => m.VehicleBrand)
                .AnyAsync(m => m.Id == modelId &&
                               m.VehicleBrandId == brandId &&
                               m.VehicleBrand.VehicleTypeId == typeId);
        }

        public async Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle)
        {
            var existingVehicle = await appDbContext.Vehicles.
                Include(x=>x.VehicleType).
                Include(x=>x.VehicleBrand).
                Include(x=>x.VehicleModel).FirstOrDefaultAsync(x => x.Id == vehicle.Id);

            existingVehicle.VehicleTypeId = vehicle.VehicleTypeId;
            existingVehicle.VehicleBrandId = vehicle.VehicleBrandId;
            existingVehicle.VehicleModelId = vehicle.VehicleModelId;
            existingVehicle.ProductionYear = vehicle.ProductionYear;
            existingVehicle.EngineCapacity = vehicle.EngineCapacity;
            existingVehicle.EnginePowerKw = vehicle.EnginePowerKw;
            existingVehicle.Weight = vehicle.Weight;
            existingVehicle.FuelType = vehicle.FuelType;
            existingVehicle.ChassisNumber = vehicle.ChassisNumber;
            existingVehicle.FirstRegistrationDate = vehicle.FirstRegistrationDate;

            await appDbContext.SaveChangesAsync();

            return existingVehicle;

        }


    }
}
