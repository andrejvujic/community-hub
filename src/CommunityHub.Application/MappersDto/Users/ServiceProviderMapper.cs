using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.MappersDto.Users
{
    public class ServiceProviderMapper : IMapper<ServiceProvider, ServiceProviderDto>
    {
        private readonly CityMapper _cityMapper = new();
        private readonly CountryMapper _countryMapper = new();

        public ServiceProviderDto MapToDto(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            return new ServiceProviderDto(
                serviceProvider.Id,
                serviceProvider.Username,
                serviceProvider.Password,
                serviceProvider.Name,
                serviceProvider.Surname,
                _cityMapper.MapToDto(serviceProvider.City),
                _countryMapper.MapToDto(serviceProvider.Country));
        }

        public ServiceProvider MapToEntity(ServiceProviderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ServiceProvider(
                dto.Id,
                dto.Username,
                dto.Password,
                dto.Name,
                dto.Surname,
                _cityMapper.MapToEntity(dto.City),
                _countryMapper.MapToEntity(dto.Country));
        }
    }
}