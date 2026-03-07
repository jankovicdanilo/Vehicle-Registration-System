using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IInsurancePricingRepository
    {
        Task<InsurancePrice> CreateAsync(InsurancePrice insurancePrice);

        Task<List<InsurancePrice>> GetAllAsync();

        Task<InsurancePrice?> GetByIdAsync(Guid id);

        Task<InsurancePrice?> GetByInsuranceIdAsync(Guid id, int kw);
    }
}
