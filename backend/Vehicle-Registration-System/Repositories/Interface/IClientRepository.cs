using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        Task<(List<ClientListItemDto> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10);

        Task<Client?> UpdateClientAsync(Client client);
    }
}
