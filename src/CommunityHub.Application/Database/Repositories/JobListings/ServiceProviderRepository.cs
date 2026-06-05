using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Repositories.JobListings;

public class ServiceProviderRepository
{
    private readonly ServiceProviderMapper _mapper = new ServiceProviderMapper();

    public ServiceProvider? GetById(long id)
    {
        var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
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
        connection.ExecuteReader(query, reader => result = _mapper.MapToEntity(reader));
        return result;
    }

    public List<ServiceProvider> GetByLocation(long cityId, long countryId)
    {
        var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
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
        connection.ExecuteReader(query, reader =>
        {
            var provider = _mapper.MapToEntity(reader);
            if (provider is not null) providers.Add(provider);
        });
        return providers;
    }

    public ServiceProvider Create(ServiceProvider provider)
    {
        using var connection = DatabaseConnection.Create();
        connection.BeginTransaction();

        var userQuery = DatabaseQuery.Create(
            @"INSERT INTO users (username, password, name, surname, user_role)
              VALUES (@username, @password, @name, @surname, @user_role)
              RETURNING id"
        ).ExtendParameters(_mapper.MapToUserParameters(provider));

        long generatedId = connection.ExecuteScalar(userQuery, Convert.ToInt64);

        var providerQuery = DatabaseQuery.Create(
            @"INSERT INTO service_providers (id, city_id, country_id)
              VALUES (@id, @city_id, @country_id)"
        )
        .AddParameter("id", generatedId)
        .AddParameter("city_id", provider.City.Id)
        .AddParameter("country_id", provider.Country.Id);

        connection.GetRowsAffectedCount(providerQuery);
        connection.CommitTransaction();

        return provider.CopyWith(id: generatedId);
    }
}