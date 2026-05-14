using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.CityFacilities
{
    public class CityFacilityRepository
    {
        private readonly CityFacilityMapper _cityFacilityMapper = new CityFacilityMapper();

        public List<CityFacility> GetByCityId(long cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    city_facility.id AS city_facility_id,
                    city_facility.name AS city_facility_name,

                    city.id AS facility_city_id,
                    city.name AS facility_city_name,

                    country.id AS facility_country_id,
                    country.name AS facility_country_name

                FROM city_facilities city_facility
                INNER JOIN cities city ON city.id = city_facility.city_id
                INNER JOIN countries country ON country.id = city.country_id
                WHERE city_facility.city_id = @city_id
                ORDER BY city_facility.name"
            ).AddParameter("city_id", cityId);

            List<CityFacility> cityFacilities = new List<CityFacility>();
            query.ExecuteReader(reader =>
            {
                var facility = _cityFacilityMapper.MapToEntity(reader);
                if (facility is not null) cityFacilities.Add(facility);
            });

            return cityFacilities;
        }
    }
}
