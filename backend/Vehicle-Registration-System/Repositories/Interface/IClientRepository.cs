using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        Task<(List<Client> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000);

        Task<Client?> UpdateClientAsync(Client client);
    }
}
