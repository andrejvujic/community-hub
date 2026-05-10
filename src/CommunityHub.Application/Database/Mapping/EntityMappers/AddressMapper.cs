using CommunityHub.Application.Domain.Addresses;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AddressMapper : EntityMapper<Address>
    {
        private readonly CountryMapper _countryMapper = new();
        private readonly CityMapper _cityMapper = new();
        private readonly StreetMapper _streetMapper = new();
        public override Address MapToEntity(IDataReader reader)
        {
            Country country = _countryMapper.MapToEntity(reader);
            City city = _cityMapper.MapToEntity(reader);
            Street street = _streetMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinal("address_id"));
            string streetNumber = reader.GetString(reader.GetOrdinal("street_number"));
            string neightborhood = reader.GetString(reader.GetOrdinal("neighborhood"));

            return new Address(id, street, streetNumber.ToString(), neightborhood, city, country);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Address entity)
        {
            return new Dictionary<string, object?>
            {
                { "street_number", entity.StreetNumber },
                { "neighborhood", entity.Neighborhood },
                { "city_id", entity.City.Id },
                { "street_id", entity.Street.Id }
            };
        }
    }
}