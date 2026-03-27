using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Common;
using System.Formats.Tar;
using VehicleRegistrationSystem.Models.DTO.Registration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class RegistrationVehicleRepository : RepositoryBase<Registration>, IRegistrationVehicleRepository
    {
        private readonly string connectionString;

        public RegistrationVehicleRepository
            (VehicleRegistrationDbContext appDbContext, 
            IRegistrationCalculatorService registrationCalculatorService,
            IConfiguration configuration) : base(appDbContext)
        {
            connectionString = configuration.GetConnectionString("VehicleRegistrationDbConnectionString");
        }
        

        public async Task<(List<RegistrationVehicleListItemDto> Items, int TotalCount)> 
            GetAllAsync(string? searchQuery = null, 
            int pageNumber = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(connectionString);

            var offset = (pageNumber - 1) * pageSize;

            var sql = @"
                with Filtered as (
                    select 
                        r.Id,
                        r.RegistrationDate,
                        r.ExpirationDate,
                        r.RegistrationPrice,
                        r.LicensePlate,
                        r.IsTemporary,
                        i.Name as Insurance,
                        vb.Name + ' ' + vm.Name as Vehicle,
                        c.FirstName + ' ' + c.LastName as Owner
                    from Registrations r
                        join Vehicles v on v.Id = r.VehicleId
                        join VehicleModels vm on vm.Id = v.VehicleModelId
                        join VehicleBrands vb on vb.Id = v.VehicleBrandId
                        join Clients c on c.Id = r.ClientId
                        join Insurances i on i.Id = r.InsuranceId
                    where (@search is null or
	                           vb.Name like '%' + @search + '%' or
	                           vm.Name like '%' + @search + '%' or
	                           c.FirstName like '%' + @search + '%' or
	                           c.LastName like '%' + @search + '%' or
	                           r.LicensePlate like '%' + @search + '%' or
                               i.Name like '%' + @search + '%')
                    )

                    select 
                        Id,
                        RegistrationDate,
                        ExpirationDate,
                        RegistrationPrice,
                        LicensePlate,
                        IsTemporary,
                        Insurance,
                        Vehicle,
                        Owner
                    from Filtered
                    order by Id
                    offset @offset rows fetch next @pageSize rows only;

                    with Filtered as(
                        select
                          r.Id
                    from Registrations r
                        join Vehicles v on v.Id = r.VehicleId
                        join VehicleModels vm on vm.Id = v.VehicleModelId
                        join VehicleBrands vb on vb.Id = v.VehicleBrandId
                        join Clients c on c.Id = r.ClientId
                        join Insurances i on i.Id = r.InsuranceId
                    where (@search is null or
	                           vb.Name like '%' + @search + '%' or
	                           vm.Name like '%' + @search + '%' or
	                           c.FirstName like '%' + @search + '%' or
	                           c.LastName like '%' + @search + '%' or
	                           r.LicensePlate like '%' + @search + '%' or
                               i.Name like '%' + @search + '%')
                    )

                    select count(*) from Filtered;
            ";

            using var multi = await connection.QueryMultipleAsync(sql, new
            {
                search = searchQuery,
                offset,
                pageSize
            });

            var items = (await multi.ReadAsync<RegistrationVehicleListItemDto>()).ToList();
            var count = await multi.ReadFirstAsync<int>();

            return (items, count);
        }

        public async Task<Registration> GetDetailedRegistrationAsync(Guid id)
        {
            return await appDbContext.Registrations
                .Include(c => c.Client)
                .Include(v => v.Vehicle)
                .ThenInclude(vt => vt.VehicleType)
                .Include(v => v.Vehicle)
                .ThenInclude(vm => vm.VehicleBrand)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(i => i.Insurance).FirstOrDefaultAsync();
        }

        public async Task<Registration?> UpdateAsync(Registration request)
        {
            var existingRegistration = await appDbContext.Registrations
                .Include(x => x.Client)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleBrand)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleModel)
                .Include(x => x.Insurance)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingRegistration == null)
                return null;

            existingRegistration.LicensePlate = request.LicensePlate;
            existingRegistration.RegistrationDate = request.RegistrationDate;
            existingRegistration.IsTemporary = request.IsTemporary;
            existingRegistration.VehicleId = request.VehicleId;
            existingRegistration.ClientId = request.ClientId;
            existingRegistration.InsuranceId = request.InsuranceId;
            existingRegistration.ExpirationDate = request.RegistrationDate.AddMonths(12);

            await appDbContext.SaveChangesAsync();

            return existingRegistration;
        }
    }
}


