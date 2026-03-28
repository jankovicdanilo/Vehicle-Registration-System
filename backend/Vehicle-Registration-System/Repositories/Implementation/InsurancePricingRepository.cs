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

        public async Task<InsurancePrice?> GetByInsuranceIdAsync(Guid id, int kw)
        {
            var exactMatch = await appDbContext.InsurancePrices
                .Where(x => x.InsuranceId == id && x.MinKw <= kw && x.MaxKw >= kw)
                .OrderByDescending(x => x.MinKw)
                .FirstOrDefaultAsync();

            if(exactMatch != null)
            {
                return exactMatch;
            }

            return await appDbContext.InsurancePrices
                .Where(x => x.InsuranceId == id)
                .OrderByDescending(x => x.MaxKw)
                .FirstOrDefaultAsync();
        }
    }
}
