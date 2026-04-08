using CommunityHub.Application.Database.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class CountryDbRepository
{
    private readonly CountryMapper _countryMapper = new CountryMapper();

    public List<Country> GetAll()
    {
        var query = PostgresQuery.Create("SELECT id as country_id, name as country_name FROM countries");
        List<Country> countries = new List<Country>();
        query.ExecuteReader(reader => countries.Add(_countryMapper.MapToEntity(reader)));
        return countries;
    }
}
