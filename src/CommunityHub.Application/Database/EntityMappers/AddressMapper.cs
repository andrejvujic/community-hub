using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class AddressMapper : EntityMapper<Address>
    {
        private readonly CountryMapper _countryMapper = new CountryMapper();
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly StreetMapper _streetMapper = new StreetMapper();
        public override Address MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            Country country = _countryMapper.MapToEntity(reader, columnNamePrefix);
            City city = _cityMapper.MapToEntity(reader, columnNamePrefix);
            Street street = _streetMapper.MapToEntity(reader, columnNamePrefix);

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("address_id", columnNamePrefix));
            string streetNumber = reader.GetString(reader.GetOrdinalPrefixed("street_number", columnNamePrefix));
            string neightborhood = reader.GetString(reader.GetOrdinalPrefixed("neighborhood", columnNamePrefix));

            return new Address(id, street, streetNumber.ToString(), neightborhood, city, country);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Address entity)
        {
            return new Dictionary<string, object?>
            {
            };
        }
    }
}
