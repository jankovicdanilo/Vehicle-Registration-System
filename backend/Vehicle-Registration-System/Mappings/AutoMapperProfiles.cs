using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Models.DTO.VehicleBrand;
using VehicleRegistrationSystem.Models.DTO.VehicleModel;
using VehicleRegistrationSystem.Models.DTO.VehicleType;
using VehicleRegistrationSystem.Models.DTO.Insurance;
using VehicleRegistrationSystem.Models.DTO.InsurancePricing;
using VehicleRegistrationSystem.Models.DTO.Registration;
using VehicleRegistrationSystem.Models.DTO.Auth;

namespace VehicleRegistrationSystem.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Vehicle, CreateVehicleRequestDto>().ReverseMap();

            CreateMap<Vehicle, UpdateVehicleDto>().ReverseMap();

            CreateMap<Vehicle, VehicleDto>().
                ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.Name)).
                ForMember(dest => dest.VehicleBrandName, opt => opt.MapFrom(src => src.VehicleBrand.Name)).
                ForMember(dest => dest.VehicleModelName, opt => 
                opt.MapFrom(src => src.VehicleModel.Name)).ReverseMap();

            CreateMap<Client, CreateClientRequestDto>().ReverseMap();

            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<Client, UpdateClientRequestDto>().ReverseMap();

            CreateMap<Insurance, CreateInsuranceRequestDto>().ReverseMap();

            CreateMap<Insurance, InsuranceDto>().ReverseMap();

            CreateMap<Insurance, UpdateInsuranceRequestDto>().ReverseMap();

            CreateMap<VehicleTypeDto, VehicleType>().ReverseMap();

            CreateMap<CreateVehicleTypeRequestDto, VehicleType>().ReverseMap();

            CreateMap<UpdateVehicleTypeRequestDto, VehicleType>().ReverseMap();

            CreateMap<VehicleBrand, VehicleBrandDto >()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.VehicleType.Category))
                .ReverseMap();

            CreateMap<VehicleBrand, UpdateVehicleBrandRequestDto>().ReverseMap();

            CreateMap<CreateVehicleBrandRequestDto, VehicleBrand>().ReverseMap();

            CreateMap<VehicleModel,VehicleModelDto>()
                .ForMember(dest=>dest.VehicleBrandName, opt=>opt.MapFrom(src=>src.VehicleBrand.Name))
                .ForMember(dest=>dest.VehicleTypeId, opt=>opt.MapFrom(src=>src.VehicleBrand.VehicleType.Id))
                .ForMember(dest=>dest.VehicleTypeName, opt=>opt.MapFrom(src=>src.VehicleBrand.VehicleType.Name))
                .ReverseMap();

            CreateMap<CreateVehicleModelRequestDto, VehicleModel>().ReverseMap();

            CreateMap<UpdateVehicleModelRequestDto, VehicleModel>().ReverseMap();

            CreateMap<Registration, RegistrationVehicleDto>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
                .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle))
                .ForMember(dest => dest.Insurance, opt=>opt.MapFrom(src=>src.Insurance))
                .ReverseMap();

            CreateMap<Registration, CreateRegistrationVehicleRequestDto>().ReverseMap();

            CreateMap<Registration, UpdateRegistrationVehicleRequestDto>().ReverseMap();

            CreateMap<UserDto,IdentityUser>().ReverseMap();

            CreateMap<LoginResponseDto, IdentityUser>().ReverseMap();

            CreateMap<LoginRequestDto, IdentityUser>().ReverseMap();

            CreateMap<InsurancePrice, CreateInsurancePriceRequestDto>().ReverseMap();

            CreateMap<InsurancePrice, InsurancePriceDto>().ReverseMap();

        }
    }
}
