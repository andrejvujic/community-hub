using System.Data;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Neighborhoods.CityFacility;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CityFacilityMapper : EntityMapper<CityFacility>
    {
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly CountryMapper _countryMapper = new CountryMapper();

        public override CityFacility? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("city_facility_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            string name = reader.GetString(reader.GetOrdinal("city_facility_name"));
            City city = _cityMapper.MapToEntity(reader.WithPrefix("facility"));
            Country country = _countryMapper.MapToEntity(reader.WithPrefix("facility"));

            return new CityFacility(name, city, country, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(CityFacility entity)
        {
            return new Dictionary<string, object?>
            {
                { "name", entity.Name },
                { "city_id", entity.City.Id }
            };
        }
    }
}
