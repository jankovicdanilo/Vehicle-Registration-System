using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public VehicleTypeRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<VehicleType> AddAsync(VehicleType vehicleType)
        {
            await appDbContext.VehicleTypes.AddAsync(vehicleType);
            await appDbContext.SaveChangesAsync();
            return vehicleType;
        }

        public async Task<VehicleType?> DeleteAsync(Guid id)
        {
            var existingVehicleType = await appDbContext.VehicleTypes.FirstOrDefaultAsync(x => x.Id==id);

            if (existingVehicleType != null)
            {
                appDbContext.VehicleTypes.Remove(existingVehicleType);
                await appDbContext.SaveChangesAsync();
                return existingVehicleType;
            }

            return null;

        }

        public async Task<List<VehicleType>> GetAllAsync()
        {
            return await appDbContext.VehicleTypes.ToListAsync();
        }

        public async Task<VehicleType?> GetByIdAsync(Guid id)
        {
            return await appDbContext.VehicleTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VehicleType?> UpdateAsync(VehicleType vehicleType)
        {
            var existingVehicleType = await appDbContext.VehicleTypes.FirstOrDefaultAsync(x => x.Id == vehicleType.Id);

            if(existingVehicleType == null)
            {
                return null;
            }

            existingVehicleType.Name = vehicleType.Name;
            existingVehicleType.Category = vehicleType.Category;

            await appDbContext.SaveChangesAsync();

            return existingVehicleType;
        }
    }
}
