using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(VehicleRegistrationDbContext appDbContext) : base(appDbContext) { }

        public async Task<(List<Client> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000)
        {
            var query = appDbContext.Clients.AsQueryable();

            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(
                x => x.FirstName.Contains(searchQuery) ||
                x.LastName.Contains(searchQuery) ||
                x.Email.Contains(searchQuery) ||
                x.IdCardNumber.Contains(searchQuery) ||
                x.NationalId.Contains(searchQuery));
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return (items, totalCount);
        }

        public async Task<Client?> UpdateClientAsync(Client client)
        {
            var existingClient = await appDbContext.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

            if (existingClient == null)
            {
                return null;
            }

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.NationalId = client.NationalId;
            existingClient.IdCardNumber = client.IdCardNumber;
            existingClient.DateOfBirth = client.DateOfBirth;
            existingClient.Email = client.Email;
            existingClient.Address = client.Address;
            existingClient.PhoneNumber = client.PhoneNumber;

            await appDbContext.SaveChangesAsync();

            return existingClient;

        }
    }
}
