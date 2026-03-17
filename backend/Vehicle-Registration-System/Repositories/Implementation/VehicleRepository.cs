using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<(List<Vehicle> Items, int TotalCount)> GetAllAsync(string? searchQuery = null,
            int pageSize = 1000, int pageNumber = 1)
        {
            var query = appDbContext.Vehicles
                .Include(x => x.VehicleType)
                .Include(x => x.VehicleBrand)
                .Include(x => x.VehicleModel)
                .AsQueryable();


            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.VehicleType.Name.Contains(searchQuery) ||
                x.VehicleBrand.Name.Contains(searchQuery) ||
                x.VehicleModel.Name.Contains(searchQuery) ||
                x.VehicleType.Name.Contains(searchQuery));
            }

            
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return (items, totalCount); 
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
