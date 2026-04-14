using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ServiceProviderMapper : EntityMapper<ServiceProvider>
    {
        private readonly UserMapper _userMapper = new UserMapper();
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly CountryMapper _countryMapper = new CountryMapper();

        public override ServiceProvider? MapToEntity(IDataReader reader)
        {
            User? baseUser = _userMapper.MapToEntity(reader);
            if (baseUser is null) return null;

            City city = _cityMapper.MapToEntity(reader.WithPrefix("provider_city"));
            Country country = _countryMapper.MapToEntity(reader.WithPrefix("provider_country"));

            return new ServiceProvider(
                baseUser.Id,
                baseUser.Username,
                baseUser.Password,
                baseUser.Name,
                baseUser.Surname,
                baseUser.Role,
                city,
                country
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(ServiceProvider entity)
        {
            var parameters = _userMapper.MapToQueryParameters(entity);
            parameters.Add("city_id", entity.City.Id);
            parameters.Add("country_id", entity.Country.Id);
            return parameters;
        }
    }
}