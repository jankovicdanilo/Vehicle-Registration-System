using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public VehicleRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            await appDbContext.Vehicles.AddAsync(vehicle);
            await appDbContext.SaveChangesAsync();

            return await appDbContext.Vehicles.
                Include(v => v.VehicleType).
                Include(v => v.VehicleBrand).
                Include(v => v.VehicleModel).
                FirstOrDefaultAsync(v => v.Id == vehicle.Id);
        }

        public async Task<Vehicle?> DeleteVehicleAsync(Guid id)
        {
            var existingVehicle = await appDbContext.Vehicles.
                Include(v=>v.VehicleType).
                Include(v=>v.VehicleBrand).
                Include(v=>v.VehicleModel).FirstOrDefaultAsync(x => x.Id == id);

            if(existingVehicle ==  null)
            {
                return null;
            }

            appDbContext.Vehicles.Remove(existingVehicle);
            await appDbContext.SaveChangesAsync();

            return existingVehicle;
            
        }

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

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await appDbContext.Vehicles.Include(x => x.VehicleType).Include(x => x.VehicleBrand).
                Include(x => x.VehicleModel).FirstOrDefaultAsync(x => x.Id == id);

            
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
