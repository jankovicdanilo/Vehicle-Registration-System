using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class InsuranceRepository : RepositoryBase<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<Insurance>> GetAllAsync()
        {
            return await appDbContext.Insurances.ToListAsync();
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
