using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories;

public class CountryRepository
{
    private readonly CountryMapper _countryMapper = new CountryMapper();

    public List<Country> GetAll()
    {
        var query = PostgresQuery.Create("SELECT id AS country_id, name AS country_name FROM countries");
        List<Country> countries = new List<Country>();
        query.ExecuteReader(reader => countries.Add(_countryMapper.MapToEntity(reader)));
        return countries;
    }

    public List<Country> Search(CountrySearchParameters searchParameters)
    {
        var query = PostgresQuery.Create(
            @"SELECT
            id AS country_id,
            name AS country_name
            FROM countries
            WHERE name ILIKE '%' || @country_name || '%'"
        )
        .ExtendParameters(searchParameters.ToQueryParameters());

        List<Country> results = new();

        query.ExecuteReader(
            reader => results.Add(
                _countryMapper.MapToEntity(reader)
            )
        );

        return results;
    }
}
