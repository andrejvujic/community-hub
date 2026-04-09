using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories;

public class CityDbRepository
{
    private readonly CityMapper _cityMapper = new CityMapper();

    public List<City> GetByCountryId(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT 
            city.id as city_id, city.name as city_name FROM cities city
            INNER JOIN countries country
            ON city.country_id = country.id
            WHERE country.id = @id"
        ).AddParameter("id", id);

        List<City> cities = new List<City>();
        query.ExecuteReader(reader => cities.Add(_cityMapper.MapToEntity(reader)));
        return cities;
    }

    public List<City> Search(CitySearchParameters searchParameters)
    {
        var query = PostgresQuery.Create(
            @"SELECT
            id AS city_id,
            name AS city_name
            FROM cities
            WHERE
            name ILIKE '%' || @city_name || '%' AND
            (country_id = @country_id OR @country_id IS NULL)"
        )
        .ExtendParameters(searchParameters.ToQueryParameters());

        List<City> results = new List<City>();
        query.ExecuteReader(reader => results.Add(_cityMapper.MapToEntity(reader)));
        return results;
    }
}
