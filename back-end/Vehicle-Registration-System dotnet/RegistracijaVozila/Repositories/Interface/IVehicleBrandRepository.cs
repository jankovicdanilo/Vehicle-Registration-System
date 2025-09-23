using RegistracijaVozila.Models.Domain;

namespace RegistracijaVozila.Repositories.Interface
{
    public interface IVehicleBrandRepository
    {
        Task<List<MarkaVozila>> ListByTypeId(Guid id);

        Task<MarkaVozila> AddAsync(MarkaVozila markaVozila);

        Task<MarkaVozila?> GetByIdAsync(Guid id);

        Task<MarkaVozila?> DeleteAsync(Guid id);

        Task<MarkaVozila?> UpdateAsync(MarkaVozila markaVozila);
    }
}
