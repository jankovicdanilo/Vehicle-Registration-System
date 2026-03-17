using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleTypeRepository : RepositoryBase<VehicleType>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<VehicleType>> GetAllAsync()
        {
            return await appDbContext.VehicleTypes.ToListAsync();
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
