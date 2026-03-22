using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;
using Microsoft.Data.SqlClient;
using Dapper;
using VehicleRegistrationSystem.Models.DTO.Client;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        private readonly string connectionString;

        public ClientRepository(VehicleRegistrationDbContext appDbContext,
            IConfiguration configuration) : base(appDbContext) 
        {
            connectionString = configuration.GetConnectionString("VehicleRegistrationDbConnectionString");
        }

        public async Task<(List<ClientListItemDto> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(connectionString);

            var offset = (pageNumber - 1) * pageSize;

            var sql = @"
                with filtered as
                    (select
                        c.Id,
                        c.FirstName,
                        c.LastName,
                        c.NationalId,
                        c.IdCardNumber,
                        c.Email,
                        c.PhoneNumber,
                        c.Address,
                        c.DateOfBirth
                    from Clients c
                    where (@search is null or
                            c.FirstName like '%' + @search + '%' or
                            c.LastName like '%' + @search + '%' or
                            c.NationalId like '%' + @search + '%' or
                            c.IdCardNumber like '%' + @search + '%' or
                            c.Email like '%' + @search + '%')
                    )

                select 
                    Id,
                    FirstName,
                    LastName,
                    NationalId,
                    IdCardNumber,
                    Email,
                    PhoneNumber,
                    Address,
                    DateOfBirth
                from Filtered
                order by Id
                offset @offset rows fetch next @pageSize rows only;

                with Filtered as(
                    select c.Id
                from Clients c
                where (@search is null or
                            c.FirstName like '%' + @search + '%' or
                            c.LastName like '%' + @search + '%' or
                            c.NationalId like '%' + @search + '%' or
                            c.IdCardNumber like '%' + @search + '%' or
                            c.Email like '%' + @search + '%')
                )

                select count(*) from Filtered;
            ";

            using var multi = await connection.QueryMultipleAsync(sql, new
            {
                search = searchQuery,
                offset,
                pageSize
            });

            var items = (await multi.ReadAsync<ClientListItemDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

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
