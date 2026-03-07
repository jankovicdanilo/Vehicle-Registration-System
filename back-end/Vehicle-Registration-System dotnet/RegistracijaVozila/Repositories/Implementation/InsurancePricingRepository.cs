using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class InsurancePricingRepository : IInsurancePricingRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public InsurancePricingRepository(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<InsurancePrice> CreateAsync(InsurancePrice insurancePrice)
        {
            await appDbContext.AddAsync(insurancePrice);
            await appDbContext.SaveChangesAsync();
            return insurancePrice;
        }

        public async Task<List<InsurancePrice>> GetAllAsync()
        {
            return await appDbContext.InsurancePrices.ToListAsync();
        }

        public async Task<InsurancePrice?> GetByIdAsync(Guid id)
        {
            return await appDbContext.InsurancePrices.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<InsurancePrice?> GetByInsuranceIdAsync(Guid id, int kw)
        {
            return await appDbContext.InsurancePrices.
                Where(x=>x.MinKw<=kw && x.MaxKw>=kw).FirstOrDefaultAsync(x=>x.InsuranceId == id);

        }
    }
}
