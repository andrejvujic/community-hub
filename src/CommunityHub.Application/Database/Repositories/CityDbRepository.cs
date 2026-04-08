using CommunityHub.Application.Database.EntityMappers;
using CommunityHub.Application.Domain;

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
}
