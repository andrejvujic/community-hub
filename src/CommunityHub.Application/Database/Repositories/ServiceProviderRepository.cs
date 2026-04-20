using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ServiceProviderRepository
{
    private readonly ServiceProviderMapper _mapper = new ServiceProviderMapper();

    public ServiceProvider? GetById(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                u.id AS user_id,
                u.username,
                u.password,
                u.name,
                u.surname,
                u.user_role,
                c.id AS provider_city_id,
                c.name AS provider_city_name,
                co.id AS provider_country_id,
                co.name AS provider_country_name
            FROM service_providers sp
            INNER JOIN users u ON u.id = sp.id
            INNER JOIN cities c ON c.id = sp.city_id
            INNER JOIN countries co ON co.id = sp.country_id
            WHERE sp.id = @id"
        ).AddParameter("id", id);

        ServiceProvider? result = null;
        query.ExecuteReader(reader => result = _mapper.MapToEntity(reader));
        return result;
    }

    public List<ServiceProvider> GetByLocation(long cityId, long countryId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                u.id AS user_id,
                u.username,
                u.password,
                u.name,
                u.surname,
                u.user_role,
                c.id AS provider_city_id,
                c.name AS provider_city_name,
                co.id AS provider_country_id,
                co.name AS provider_country_name
            FROM service_providers sp
            INNER JOIN users u ON u.id = sp.id
            INNER JOIN cities c ON c.id = sp.city_id
            INNER JOIN countries co ON co.id = sp.country_id
            WHERE sp.city_id = @city_id AND sp.country_id = @country_id"
        )
        .AddParameter("city_id", cityId)
        .AddParameter("country_id", countryId);

        var providers = new List<ServiceProvider>();
        query.ExecuteReader(reader =>
        {
            var provider = _mapper.MapToEntity(reader);
            if (provider is not null) providers.Add(provider);
        });
        return providers;
    }

    public ServiceProvider Create(ServiceProvider provider)
    {
        // INSERT u tabelu users i dobijamo generisani ID
        var userQuery = PostgresQuery.Create(
            @"INSERT INTO users (username, password, name, surname, user_role)
              VALUES (@username, @password, @name, @surname, @user_role)
              RETURNING id"
        ).ExtendParameters(_mapper.MapToUserParameters(provider));

        long generatedId = userQuery.ExecuteScalar<long>(value => Convert.ToInt64(value));

        // INSERT u tabelu service_providers sa istim ID-om
        PostgresQuery.Create(
            @"INSERT INTO service_providers (id, city_id, country_id)
              VALUES (@id, @city_id, @country_id)"
        )
        .AddParameter("id", generatedId)
        .AddParameter("city_id", provider.City.Id)
        .AddParameter("country_id", provider.Country.Id)
        .GetRowsAffectedCount();

        return provider.CopyWith(id: generatedId);
    }
}