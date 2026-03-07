using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IClientRepository
    {
        Task<Client> AddClientAsync(Client client);

        Task<(List<Client> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000);

        Task<Client?> GetClientByIdAsync(Guid id);

        Task<Client?> UpdateClientAsync(Client client);

        Task<Client?> DeleteClientAsync(Guid id);
    }
}
