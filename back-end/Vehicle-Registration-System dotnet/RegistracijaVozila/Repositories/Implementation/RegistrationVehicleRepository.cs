using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;

namespace VehicleRegistrationSystem.Repositories.Implementation
{
    public class RegistrationVehicleRepository : IRegistrationVehicleRepository
    {
        private readonly VehicleRegistrationDbContext appDbContext;

        public RegistrationVehicleRepository(VehicleRegistrationDbContext appDbContext, 
            IRegistrationCalculatorService registrationCalculatorService)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Registration> AddRegistrationAsync(Registration request)
        {
            await appDbContext.Registrations.AddAsync(request);
            await appDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<(List<Registration> Items, int TotalCount)> GetAllAsync(string? searchQuery = null, 
            int pageNumber = 1, int pageSize = 1000)
        {
            var query = appDbContext.Registrations
                .Include(x => x.Client)
                .Include(x => x.Vehicle)
                .Include(x => x.Vehicle.VehicleType)
                .Include(x => x.Vehicle.VehicleBrand)
                .Include(x=>x.Insurance)
                .Include(x => x.Vehicle.VehicleModel).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x =>
                    x.Client.FirstName.Contains(searchQuery) ||
                    x.Client.IdCardNumber.Contains(searchQuery) ||
                    x.Client.Email.Contains(searchQuery) ||
                    x.Client.NationalId.Contains(searchQuery) ||
                    x.Client.LastName.Contains(searchQuery) ||
                    x.Vehicle.VehicleBrand.Name.Contains(searchQuery) ||
                    x.Vehicle.VehicleModel.Name.Contains(searchQuery) ||
                    x.Vehicle.VehicleType.Name.Contains(searchQuery) ||
                    x.Insurance.Name.Contains(searchQuery) ||
                    x.LicensePlate.Contains(searchQuery)
                );
            }

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return (items, totalCount);
        }

        public async Task<Registration?> GetByIdAsync(Guid id)
        {
            return await appDbContext.Registrations
                .Include(x => x.Client)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleBrand)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleModel)
                .Include(x => x.Insurance)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Registration?> DeleteAsync(Guid id)
        {
            var existingRegistration = await appDbContext.Registrations
                .Include(x => x.Client)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleBrand)
                .Include(x => x.Vehicle).ThenInclude(v => v.VehicleModel)
                .Include(x => x.Insurance)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegistration == null)
                return null;

            appDbContext.Registrations.Remove(existingRegistration);

            await appDbContext.SaveChangesAsync();

            return existingRegistration;
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


