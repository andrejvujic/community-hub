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
                c.name AS provider_city_name
            FROM service_providers sp
            INNER JOIN users u ON u.id = sp.id
            INNER JOIN cities c ON c.id = sp.city_id
            WHERE sp.id = @id"
        ).AddParameter("id", id);

        ServiceProvider? result = null;
        query.ExecuteReader(reader => result = _mapper.MapToEntity(reader));
        return result;
    }
}