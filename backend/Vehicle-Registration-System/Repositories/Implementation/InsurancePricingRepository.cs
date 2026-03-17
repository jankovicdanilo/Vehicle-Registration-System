using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class InsurancePricingRepository : RepositoryBase<InsurancePrice>, IInsurancePricingRepository
    {
        public InsurancePricingRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<InsurancePrice>> GetAllAsync()
        {
            return await appDbContext.InsurancePrices.ToListAsync();
        }

        public async Task<InsurancePrice> GetByInsuranceIdAsync(Guid id, int kw)
        {
            return await appDbContext.InsurancePrices.
                Where(x => x.MinKw <= kw && x.MaxKw >= kw).FirstOrDefaultAsync(x => x.InsuranceId == id);
        }
    }
}
