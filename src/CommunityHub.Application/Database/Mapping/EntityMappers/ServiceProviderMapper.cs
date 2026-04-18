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
                city,
                country
            );
        }

        // Parametri za INSERT/UPDATE u tabelu users
        public Dictionary<string, object?> MapToUserParameters(ServiceProvider entity)
        {
            return new Dictionary<string, object?>
            {
                { "username", entity.Username },
                { "password", entity.Password },
                { "name", entity.Name },
                { "surname", entity.Surname },
                { "user_role", entity.Role }
            };
        }

        // Parametri za INSERT/UPDATE u tabelu service_providers
        public Dictionary<string, object?> MapToProviderParameters(ServiceProvider entity)
        {
            return new Dictionary<string, object?>
            {
                { "id", entity.Id },
                { "city_id", entity.City.Id },
                { "country_id", entity.Country.Id }
            };
        }

        // Implementacija kombinovanja oba skupa parametara
        public override Dictionary<string, object?> MapToQueryParameters(ServiceProvider entity)
        {
            var parameters = MapToUserParameters(entity);
            foreach (var kvp in MapToProviderParameters(entity))
                parameters[kvp.Key] = kvp.Value;
            return parameters;
        }
    }
}