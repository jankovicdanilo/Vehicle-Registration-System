using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class VehicleBrandRepository: IVehicleBrandRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public VehicleBrandRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<VehicleBrand> AddAsync(VehicleBrand vehicleBrand)
        {
            await appDbContext.VehicleBrands.AddAsync(vehicleBrand);
            await appDbContext.SaveChangesAsync();

            await appDbContext.Entry(vehicleBrand).Reference(m => m.VehicleType).LoadAsync();

            return vehicleBrand;

        }

        public async Task<VehicleBrand?> DeleteAsync(Guid id)
        {
            var vehicleBrand = await appDbContext.VehicleBrands.
                Include(x=>x.VehicleType).FirstOrDefaultAsync(x => x.Id == id);

            if(vehicleBrand == null)
            {
                return null;
            }

            appDbContext.VehicleBrands.Remove(vehicleBrand);
            await appDbContext.SaveChangesAsync();

            return vehicleBrand;
        }

        public async Task<VehicleBrand?> GetByIdAsync(Guid id)
        {
            var vehicleBrand = await appDbContext.VehicleBrands.FirstOrDefaultAsync(x=>x.Id==id);

            if (vehicleBrand == null)
            {
                return null;
            }

            await appDbContext.Entry(vehicleBrand).Reference(m => m.VehicleType).LoadAsync();

            return vehicleBrand;
        }

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
