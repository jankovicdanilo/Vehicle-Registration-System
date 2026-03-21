using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.VehicleType;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IMapper mapper;
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IVehicleBrandRepository vehicleBrandRepository;

        public VehicleTypeService(IMapper mapper, IVehicleTypeRepository vehicleTypeRepository,
            IVehicleBrandRepository vehicleBrandRepository)
        {
            this.mapper = mapper;
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.vehicleBrandRepository = vehicleBrandRepository;
        }

        public async Task<RepositoryResult<bool>> 
            ValidateVehicleTypeCreateRequestAsync(CreateVehicleTypeRequestDto request)
        {

            var existingCategory = await vehicleTypeRepository.ExistsAsync(x =>
                x.Category == request.Category);


            if (existingCategory)
            {
                return RepositoryResult<bool>.Fail("VEHICLE_TYPE_CATEGORY_EXISTS: " +
                    "The type of category already exists");
            }

            var existingName = await vehicleTypeRepository.ExistsAsync(x => x.Name == request.Name);

            if (existingName)
            {
                return RepositoryResult<bool>.Fail("VEHICLE_TYPE_NAME_EXISTS: Vehicle type already exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }



        public async Task<RepositoryResult<VehicleTypeDto>> 
            CreateVehicleTypeAsync(CreateVehicleTypeRequestDto request)
        {
            var validationResult = await ValidateVehicleTypeCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleTypeDto>.Fail(validationResult.Message);
            }

            var vehicleDomain = mapper.Map<VehicleType>(request);

            vehicleDomain = await vehicleTypeRepository.AddAsync(vehicleDomain);

            var response = mapper.Map<VehicleTypeDto>(vehicleDomain);

            return RepositoryResult<VehicleTypeDto>.Ok
                (response, "New type of vehicle has successfully been created!");
        }

        public async Task<RepositoryResult<bool>> ValidateVehicleTypeDeleteRequestAsync(Guid id)
        {
            var hasBrands = await vehicleBrandRepository.ExistsAsync(x => x.VehicleTypeId == id);

            if (hasBrands)
            {
                return RepositoryResult<bool>.Fail
                    ("TYPE_HAS_BRANDS: Vehicle type cannot be deleted because it has associated brands.");
            }

            var exists = await vehicleTypeRepository.ExistsAsync(x => x.Id == id);

            if (!exists)
            {
                return RepositoryResult<bool>.Fail
                    ($"VEHICLE_TYPE_NOT_FOUND: Vehicle type with the id {id} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleTypeDto>> DeleteVehicleTypeAsync(Guid id)
        {
            var validationResult = await ValidateVehicleTypeDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleTypeDto>.Fail(validationResult.Message);
            }

            var vehicleTypeDomain = await vehicleTypeRepository.DeleteAsync(id);

            var reponse = mapper.Map<VehicleTypeDto>(vehicleTypeDomain);

            return RepositoryResult<VehicleTypeDto>.Ok(reponse, "Vehicle type has successfully been deleted!");
        }

        public async Task<RepositoryResult<bool>> 
            ValidateVehicleTypeUpdateRequestAsync(UpdateVehicleTypeRequestDto request)
        {
            var exists = await vehicleTypeRepository.ExistsAsync(x => x.Id == request.Id);

            if (!exists)
            {
                return RepositoryResult<bool>.Fail
                    ($"VEHICLE_TYPE_NOT_FOUND: Vehicle with the id {request.Id} doesnt exist");
            }

            var existingCategory = await vehicleTypeRepository.ExistsAsync(x =>
                x.Category == request.Category && x.Id!=request.Id);


            if (existingCategory)
            {
                return RepositoryResult<bool>.Fail
                    ("VEHICLE_TYPE_CATEGORY_EXISTS: The type of category already exists");
            }

            var existingName = await vehicleTypeRepository.ExistsAsync
                (x => x.Name == request.Name && x.Id!=request.Id);

            if (existingName)
            {
                return RepositoryResult<bool>.Fail("VEHICLE_TYPE_NAME_EXISTS: Vehicle type already exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleTypeDto>> 
            UpdateVehicleTypeAsync(UpdateVehicleTypeRequestDto request)
        {
            var validationResult = await ValidateVehicleTypeUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleTypeDto>.Fail(validationResult.Message);
            }

            var vehicleTypeDomain = mapper.Map<VehicleType>(request);

            vehicleTypeDomain = await vehicleTypeRepository.UpdateAsync(vehicleTypeDomain);

            var response = mapper.Map<VehicleTypeDto>(vehicleTypeDomain);

            return RepositoryResult<VehicleTypeDto>.Ok(response, "Vehicle type has successfully been updated!");
        }

        public async Task<RepositoryResult<List<VehicleTypeDto>>> GetAllAsync()
        {
            var vehicleTypesDomain = await vehicleTypeRepository.GetAllAsync();

            var response = mapper.Map<List<VehicleTypeDto>>(vehicleTypesDomain);

            return RepositoryResult<List<VehicleTypeDto>>.Ok(response);
        }

        public async Task<RepositoryResult<bool>> ValidateVehicleTypeGetByIdAsync(Guid id)
        {
            if(!await vehicleTypeRepository.ExistsAsync(x=>x.Id == id))
            {
                return RepositoryResult<bool>.Fail
                    ($"VEHICLE_TYPE_NOT_FOUND: Vehicle type with the Id {id} was not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<VehicleTypeDto>> GetById(Guid id)
        {
            var validationResult = await ValidateVehicleTypeGetByIdAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<VehicleTypeDto>.Fail(validationResult.Message);
            }

            var vehicleTypeDomain = await vehicleTypeRepository.GetByIdAsync(id);

            var response = mapper.Map<VehicleTypeDto>(vehicleTypeDomain);

            return RepositoryResult<VehicleTypeDto>.Ok(response);
        }

    }
}
