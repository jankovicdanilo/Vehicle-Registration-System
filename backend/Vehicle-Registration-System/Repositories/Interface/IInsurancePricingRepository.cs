using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;
using VehicleRegistrationSystem.Repositories.Implementation;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IInsurancePricingRepository : IRepositoryBase<InsurancePrice>
    {
        Task<List<InsurancePrice>> GetAllAsync();

        Task<InsurancePrice> GetByInsuranceIdAsync(Guid id, int kw);
    }
}
