using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories.Shared.Addresses;

public class CityRepository : ICityRepository
{
    private readonly CityMapper _cityMapper = new();

    public City? GetById(long id)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
            @"SELECT 
            city.id as city_id, city.name as city_name FROM cities city
            WHERE city.id = @id"
        ).AddParameter("id", id);

        City? city = null;

        connection.ExecuteReader(
            query,
            reader => city = _cityMapper.MapToEntity(reader)
        );

        return city;
    }

    public List<City> GetByCountryId(long id)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
            @"SELECT 
            city.id as city_id, city.name as city_name FROM cities city
            INNER JOIN countries country
            ON city.country_id = country.id
            WHERE country.id = @id"
        ).AddParameter("id", id);

        List<City> cities = new List<City>();
        connection.ExecuteReader(query, reader => cities.Add(_cityMapper.MapToEntity(reader)));
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

        List<City> results = new();

        query.ExecuteReader(
            reader => results.Add(
                _cityMapper.MapToEntity(reader)
            )
        );

        return results;
    }
}