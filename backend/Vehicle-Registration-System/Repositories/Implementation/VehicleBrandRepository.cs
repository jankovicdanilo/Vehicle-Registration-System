using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleBrandRepository: RepositoryBase<VehicleBrand>, IVehicleBrandRepository
    {
        public VehicleBrandRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<VehicleBrand>> ListByTypeId(Guid id)
        {
            return await appDbContext.VehicleBrands
                .Include(x=>x.VehicleType)
                .Where(x => x.VehicleTypeId == id).ToListAsync();
        }

        public async Task<VehicleBrand?> UpdateAsync(VehicleBrand vehicleBrand)
        {
            var existingVehicleBrand = await appDbContext.VehicleBrands.
                Include(x=>x.VehicleType).FirstOrDefaultAsync
                (x => x.Id == vehicleBrand.Id);

            if(existingVehicleBrand == null)
            {
                return null;
            }

            existingVehicleBrand.Name = vehicleBrand.Name;
            existingVehicleBrand.VehicleTypeId = vehicleBrand.VehicleTypeId;

            await appDbContext.SaveChangesAsync();

            return existingVehicleBrand;
        }
    }
}
