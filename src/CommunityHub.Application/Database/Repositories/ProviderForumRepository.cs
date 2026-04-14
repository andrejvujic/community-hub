using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ProviderForumRepository
{
    private readonly ProviderForumMapper _mapper = new ProviderForumMapper();

    public List<ProviderForum> GetVisibleForProvider(long providerId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                pf.id AS forum_id,
                pf.title AS forum_title,
                pf.description AS forum_description,
                pf.is_closed,
                pf.created_at,
                u.id AS user_id,
                u.username,
                u.password,
                u.name,
                u.surname,
                u.user_role,
                pc.id AS provider_city_id,
                pc.name AS provider_city_name
            FROM provider_forums pf
            INNER JOIN service_providers sp ON sp.id = pf.created_by_id
            INNER JOIN users u ON u.id  = sp.id
            INNER JOIN cities pc ON pc.id = sp.city_id
            WHERE pf.is_closed = FALSE
               OR pf.created_by_id = @provider_id
            ORDER BY pf.created_at DESC"
        ).AddParameter("provider_id", providerId);

        var forums = new List<ProviderForum>();
        query.ExecuteReader(reader =>
        {
            var forum = _mapper.MapToEntity(reader);
            if (forum is not null) forums.Add(forum);
        });
        return forums;
    }

    public ProviderForum Create(ProviderForum forum)
    {
        var query = PostgresQuery.Create(
            @"INSERT INTO provider_forums (title, description, is_closed, created_at, created_by_id)
              VALUES (@title, @description, @is_closed, @created_at, @created_by_id)
              RETURNING id"
        ).ExtendParameters(_mapper.MapToQueryParameters(forum));

        long newId = query.ExecuteScalar<long>(v => Convert.ToInt64(v));
        return forum.CopyWith(id: newId);
    }

    public List<ProviderForum> Search(string titleOrDescription, long currentProviderId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                pf.id AS forum_id,
                pf.title AS forum_title,
                pf.description AS forum_description,
                pf.is_closed,
                pf.created_at,
                u.id AS user_id,
                u.username,
                u.password,
                u.name,
                u.surname,
                u.user_role,
                pc.id AS provider_city_id,
                pc.name AS provider_city_name
            FROM provider_forums pf
            INNER JOIN service_providers sp ON sp.id = pf.created_by_id
            INNER JOIN users u ON u.id = sp.id
            INNER JOIN cities pc ON pc.id = sp.city_id
            WHERE (pf.is_closed = FALSE OR pf.created_by_id = @provider_id)
              AND (pf.title ILIKE '%' || @search || '%'
                OR pf.description ILIKE '%' || @search || '%')
            ORDER BY pf.created_at DESC"
        )
        .AddParameter("provider_id", currentProviderId)
        .AddParameter("search", titleOrDescription);

        var forums = new List<ProviderForum>();
        query.ExecuteReader(reader =>
        {
            var forum = _mapper.MapToEntity(reader);
            if (forum is not null) forums.Add(forum);
        });
        return forums;
    }
}