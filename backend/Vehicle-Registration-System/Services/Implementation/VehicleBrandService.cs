using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.VehicleBrand;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class VehicleBrandService : IVehicleBrandService
    {
        private readonly IVehicleBrandRepository vehicleBrandRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IMapper mapper;

        public VehicleBrandService(IVehicleBrandRepository vehicleBrandRepository,
            IVehicleTypeRepository vehicleTypeRepository,
            IVehicleModelRepository vehicleModelRepository,
            IVehicleRepository vehicleRepository,
            IMapper mapper)
        {
            this.vehicleBrandRepository = vehicleBrandRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.vehicleModelRepository = vehicleModelRepository;
            this.vehicleRepository = vehicleRepository;
            this.mapper = mapper;
        }

        public async Task<RepositoryResult<bool>> 
            ValidateVehicleBrandCreateRequestAsync(CreateVehicleBrandRequestDto request)
        {
            var existingBrand = await vehicleBrandRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.ToLower() &&
                x.VehicleTypeId == request.VehicleTypeId);

            if (existingBrand)
            {
                return RepositoryResult<bool>.Fail("VEHICLE_BRAND_EXISTS: " +
                    "Brand already exists for the given vehicle type");
            }

            if(!await vehicleTypeRepository.ExistsAsync(x=>x.Id == request.VehicleTypeId))
            {
                return RepositoryResult<bool>.Fail($"VEHICLE_TYPE_NOT_FOUND: Vehicle type with the id " +
                    $"{request.VehicleTypeId} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleBrandDto>> 
            CreateVehicleBrand(CreateVehicleBrandRequestDto request)
        {
            var validationResult = await ValidateVehicleBrandCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleBrandDto>.Fail(validationResult.Message);
            }

            var vehicleBrandDomain = mapper.Map<VehicleBrand>(request);
            var result = await vehicleBrandRepository.AddAsync(vehicleBrandDomain);

            var response = mapper.Map<VehicleBrandDto>(result);

            return RepositoryResult<VehicleBrandDto>.Ok
                (response, "New vehicle brand has successfully been created!");
        }

        public async Task<RepositoryResult<bool>> ValidateVehicleBrandDeleteRequestAsync(Guid id)
        {
            if(await vehicleModelRepository.ExistsAsync(x=>x.VehicleBrandId == id))
            {
                return RepositoryResult<bool>.Fail("VEHICLE_BRAND_HAS_MODELS: " +
                    " Vehicle brand has models and can't be deleted");
            }

            if(!await vehicleBrandRepository.ExistsAsync(x=>x.Id == id))
            {
                return RepositoryResult<bool>.Fail($"VEHICLE_BRAND_NOT_FOUND: " +
                    $"Vehicle brand with Id {id} was not found");
            }

            if(await vehicleRepository.ExistsAsync(x=>x.VehicleBrandId == id))
            {
                return RepositoryResult<bool>.Fail("VEHICLE_BRAND_IN_USE: " +
                    "Cannot delete brand because it's assigned to existing vehicles");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleBrandDto>> DeleteVehicleBrand(Guid id)
        {
            var validationResult = await ValidateVehicleBrandDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleBrandDto>.Fail(validationResult.Message);
            }

            var vehicleDomain = await vehicleBrandRepository.DeleteAsync(id);

            var response = mapper.Map<VehicleBrandDto>(vehicleDomain);

            return RepositoryResult<VehicleBrandDto>.Ok(response, "Vehicle brand has successfully been deleted!");
        }

        public async Task<RepositoryResult<bool>> 
            ValidateVehicleBrandUpdateRequestAsync(UpdateVehicleBrandRequestDto request)
        {
            if(!await vehicleBrandRepository.ExistsAsync(x => x.Id == request.Id))
            {
                return RepositoryResult<bool>.Fail($"VEHICLE_BRAND_NOT_FOUND: " +
                    $"Vehicle brand with the id {request.Id} doesnt exist");
            }

            var existingBrand = await vehicleBrandRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.ToLower() &&
                x.VehicleTypeId == request.VehicleTypeId && x.Id!=request.Id);

            if(existingBrand)
            {
                return RepositoryResult<bool>.Fail("VEHICLE_BRAND_EXISTS: " +
                    "Brand already exists for the given vehicle type");
            }

            if (!await vehicleTypeRepository.ExistsAsync(x => x.Id == request.VehicleTypeId))
            {
                return RepositoryResult<bool>.Fail($"VEHICLE_TYPE_NOT_FOUND: Vehicle type with the id " +
                    $"{request.VehicleTypeId} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleBrandDto>> 
            UpdateVehicleBrand(UpdateVehicleBrandRequestDto request)
        {
            var validationResult = await ValidateVehicleBrandUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleBrandDto>.Fail(validationResult.Message);
            }

            var vehicleBrandDomain = mapper.Map<VehicleBrand>(request);

            var updatedBrandVehicle = await vehicleBrandRepository.UpdateAsync(vehicleBrandDomain);

            var response = mapper.Map<VehicleBrandDto>(updatedBrandVehicle);

            return RepositoryResult<VehicleBrandDto>.Ok(response, "Vehicle brand has been successfully updated!");
        }

        public async Task<RepositoryResult<bool>> ValidateVehicleBrandGetListByTypeAsync(Guid id)
        {
            if(!await vehicleBrandRepository.ExistsAsync(x=>x.VehicleTypeId ==  id))
            {
                return RepositoryResult<bool>.Fail($"INCORRECT TYPE ID: Vehicle type with the id {id}" +
                    $" doesn't exist!");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<List<VehicleBrandDto>>> GetListByType(Guid id)
        {
            var validationResult = await ValidateVehicleBrandGetListByTypeAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<List<VehicleBrandDto>>.Fail(validationResult.Message);
            }

            var vehicleBrandsDomain = await vehicleBrandRepository.ListByTypeId(id);

            var response = mapper.Map<List<VehicleBrandDto>>(vehicleBrandsDomain);

            return RepositoryResult<List<VehicleBrandDto>>.Ok(response);
        }

        public async Task<RepositoryResult<bool>> ValidateVehicleBrandGetByIdAsync(Guid id)
        {
            if (!await vehicleBrandRepository.ExistsAsync(x => x.Id == id))
            {
                return RepositoryResult<bool>.Fail($"INCORRECT BRAND ID: Vehicle brand with the  id {id}" +
                    $" doesn't exist!");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleBrandDto>> GetById(Guid id)
        {
            var validationResult = await ValidateVehicleBrandGetByIdAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleBrandDto>.Fail(validationResult.Message);
            }

            var vehicleBrandDomain = await vehicleBrandRepository.GetByIdAsync(id);

            var response = mapper.Map<VehicleBrandDto>(vehicleBrandDomain);

            return RepositoryResult<VehicleBrandDto>.Ok(response);
        }
    }
}






