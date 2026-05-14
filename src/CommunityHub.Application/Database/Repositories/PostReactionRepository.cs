using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.ServiceForums;

namespace CommunityHub.Application.Database.Repositories;

public class PostReactionRepository
{
    private readonly PostReactionMapper _mapper = new PostReactionMapper();

    // PostReactionMapper ugnježdava ForumPostMapper (standardni aliasi)
    // i ServiceProviderMapper za reaktora sa prefiksom "reactor_".
    // ForumPostMapper ugnježdava ServiceProviderMapper za autora (bez prefiksa).
    // Zato SQL mora da pruži:
    // - reakciju: reaction_id, is_upvote,
    // - post (kroz ForumPostMapper),
    // - autora posta (ServiceProviderMapper bez prefiksa),
    // - autora reakcije (ServiceProviderMapper sa prefiksom "reactor").

    private const string ReactionSelectColumns = @"
        pr.id AS reaction_id,
        pr.is_upvote,
        fp.id AS post_id,
        fp.forum_id AS post_forum_id,
        fp.content AS post_content,
        fp.posted_at,
        COUNT(all_r.id) FILTER (WHERE all_r.is_upvote = TRUE)  AS upvote_count,
        COUNT(all_r.id) FILTER (WHERE all_r.is_upvote = FALSE) AS downvote_count,
        au.id AS user_id,
        au.username,
        au.password,
        au.name,
        au.surname,
        au.user_role,
        ac.id AS provider_city_id,
        ac.name AS provider_city_name,
        aco.id AS provider_country_id,
        aco.name AS provider_country_name,
        ru.id AS reactor_user_id,
        ru.username AS reactor_username,
        ru.password AS reactor_password,
        ru.name AS reactor_name,
        ru.surname AS reactor_surname,
        ru.user_role AS reactor_user_role,
        rc.id AS reactor_provider_city_id,
        rc.name AS reactor_provider_city_name,
        rco.id AS reactor_provider_country_id,
        rco.name AS reactor_provider_country_name";

    private const string ReactionJoins = @"
        INNER JOIN forum_posts fp ON fp.id = pr.post_id
        INNER JOIN service_providers asp ON asp.id = fp.author_id
        INNER JOIN users au ON au.id = asp.id
        INNER JOIN cities ac ON ac.id = asp.city_id
        INNER JOIN countries aco ON aco.id = asp.country_id
        INNER JOIN service_providers rsp ON rsp.id = pr.reactor_id
        INNER JOIN users ru ON ru.id = rsp.id
        INNER JOIN cities rc ON rc.id = rsp.city_id
        INNER JOIN countries rco ON rco.id = rsp.country_id
        LEFT JOIN post_reactions all_r ON all_r.post_id = fp.id";

    private const string ReactionGroupBy = @"
        GROUP BY pr.id, fp.id, au.id, ac.id, aco.id, ru.id, rc.id, rco.id";

    public PostReaction? GetByPostAndReactor(long postId, long reactorId)
    {
        var query = PostgresQuery.Create($@"
            SELECT {ReactionSelectColumns}
            FROM post_reactions pr
            {ReactionJoins}
            WHERE pr.post_id = @post_id
              AND pr.reactor_id = @reactor_id
            {ReactionGroupBy}"
        )
        .AddParameter("post_id", postId)
        .AddParameter("reactor_id", reactorId);

        PostReaction? result = null;
        query.ExecuteReader(reader => result = _mapper.MapToEntity(reader));
        return result;
    }

    public PostReaction Create(PostReaction reaction)
    {
        long newId = PostgresQuery.Create(@"
            INSERT INTO post_reactions (is_upvote, post_id, reactor_id)
            VALUES (@is_upvote, @post_id, @reactor_id)
            RETURNING id"
        )
        .ExtendParameters(_mapper.MapToQueryParameters(reaction))
        .ExecuteScalar<long>(v => Convert.ToInt64(v));

        return reaction.CopyWith(id: newId);
    }

    public void Update(long reactionId, bool isUpvote)
    {
        PostgresQuery.Create(@"
            UPDATE post_reactions
            SET is_upvote = @is_upvote
            WHERE id = @reaction_id"
        )
        .AddParameter("reaction_id", reactionId)
        .AddParameter("is_upvote", isUpvote)
        .GetRowsAffectedCount();
    }

    public void Delete(long reactionId)
    {
        PostgresQuery.Create(@"
            DELETE FROM post_reactions
            WHERE id = @reaction_id"
        )
        .AddParameter("reaction_id", reactionId)
        .GetRowsAffectedCount();
    }
}