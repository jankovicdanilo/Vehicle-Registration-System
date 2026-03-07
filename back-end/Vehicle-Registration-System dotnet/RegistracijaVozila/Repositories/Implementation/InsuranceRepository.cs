using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public InsuranceRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Insurance> CreateInsuranceAsync(Insurance insurance)
        {
            await appDbContext.Insurances.AddAsync(insurance);
            await appDbContext.SaveChangesAsync();
            return insurance;

        }

        public async Task<Insurance?> DeleteAsync(Guid id)
        {
            var insurance = await appDbContext.Insurances.FirstOrDefaultAsync(x=>x.Id == id);

            appDbContext.Insurances.Remove(insurance);
            await appDbContext.SaveChangesAsync();

            return insurance;
        }

        public async Task<List<Insurance>> GetAllAsync()
        {
            return await appDbContext.Insurances.ToListAsync();
        }

        public async Task<Insurance?> GetInsuranceByIdAsync(Guid id)
        {
            return await appDbContext.Insurances.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Insurance?> UpdateAsync(Insurance request)
        {
            var existingInsurance = await appDbContext.Insurances.FirstOrDefaultAsync(x => x.Id == request.Id);

            existingInsurance.Name = request.Name;

            await appDbContext.SaveChangesAsync();

            return existingInsurance;
        }
    }
}
