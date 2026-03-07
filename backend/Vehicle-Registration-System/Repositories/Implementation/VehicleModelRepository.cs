using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleModelRepository : IVehicleModelRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public VehicleModelRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<VehicleModel>> ListByBrandId(Guid id)
        {
            return await appDbContext.VehicleModels
                .Include(x=>x.VehicleBrand)
                .ThenInclude(x=>x.VehicleType)
                .Where(x => x.VehicleBrandId == id).ToListAsync();
        }

        public async Task<VehicleModel> AddAsync(VehicleModel vehicleModel)
        {
            await appDbContext.VehicleModels.AddAsync(vehicleModel);
            await appDbContext.SaveChangesAsync();
            await appDbContext.Entry(vehicleModel).Reference(m => m.VehicleBrand).LoadAsync();
            return vehicleModel;
        }

        public async Task<VehicleModel?> DeleteAsync(Guid id)
        {
            var existingVehicle = await appDbContext.VehicleModels
                .Include(x=>x.VehicleBrand)
                .ThenInclude(x=>x.VehicleType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingVehicle == null)
            {
                return null;
            }

            appDbContext.VehicleModels.Remove(existingVehicle);
            await appDbContext.SaveChangesAsync();
            return existingVehicle;
        }

        public async Task<VehicleModel?> GetByIdAsync(Guid id)
        {
            return await appDbContext.VehicleModels
                .Include(x=>x.VehicleBrand)
                .ThenInclude(x=>x.VehicleType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VehicleModel?> UpdateAsync(VehicleModel vehicleModel)
        {
            var existingVehicle = await appDbContext.VehicleModels
                 .Include(x=>x.VehicleBrand)
                 .ThenInclude(x=>x.VehicleType)
                 .FirstOrDefaultAsync(x => x.Id == vehicleModel.Id);

            if (existingVehicle != null)
            {
                existingVehicle.Name = vehicleModel.Name;
                existingVehicle.VehicleBrandId = vehicleModel.VehicleBrandId;
                await appDbContext.SaveChangesAsync();
                return existingVehicle;
            }

            return null;
        }
    }
}
