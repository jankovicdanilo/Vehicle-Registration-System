using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleModelRepository : RepositoryBase<VehicleModel>, IVehicleModelRepository
    {
        public VehicleModelRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }
        
        public async Task<List<VehicleModel>> ListByBrandId(Guid id)
        {
            return await appDbContext.VehicleModels
                .Include(x=>x.VehicleBrand)
                .ThenInclude(x=>x.VehicleType)
                .Where(x => x.VehicleBrandId == id).ToListAsync();
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
