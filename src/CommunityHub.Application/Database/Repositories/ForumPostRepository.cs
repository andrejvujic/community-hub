using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ForumPostRepository
{
    private readonly ForumPostMapper _mapper = new ForumPostMapper();

    // upvote_count/downvote_count se računaju agregacijom,
    // author se mapira kroz standardne aliase ServiceProviderMapper-a.

    private const string PostSelectColumns = @"
        fp.id AS post_id,
        fp.forum_id AS post_forum_id,
        fp.content AS post_content,
        fp.posted_at,
        COUNT(r.id) FILTER (WHERE r.is_upvote = TRUE) AS upvote_count,
        COUNT(r.id) FILTER (WHERE r.is_upvote = FALSE) AS downvote_count,
        u.id AS user_id,
        u.username,
        u.password,
        u.name,
        u.surname,
        u.user_role,
        c.id AS provider_city_id,
        c.name AS provider_city_name,
        co.id AS provider_country_id,
        co.name AS provider_country_name";

    private const string PostJoins = @"
        INNER JOIN service_providers sp ON sp.id = fp.author_id
        INNER JOIN users u ON u.id  = sp.id
        INNER JOIN cities c ON c.id  = sp.city_id
        INNER JOIN countries co ON co.id = sp.country_id
        LEFT JOIN post_reactions r ON r.post_id = fp.id";

    private const string PostGroupBy = @"
        GROUP BY fp.id, u.id, c.id, co.id";

    public List<ForumPost> GetByForumId(long forumId)
    {
        var query = PostgresQuery.Create($@"
            SELECT {PostSelectColumns}
            FROM forum_posts fp
            {PostJoins}
            WHERE fp.forum_id = @forum_id
            {PostGroupBy}
            ORDER BY fp.posted_at ASC"
        ).AddParameter("forum_id", forumId);

        var posts = new List<ForumPost>();
        query.ExecuteReader(reader =>
        {
            var post = _mapper.MapToEntity(reader);
            if (post is not null) posts.Add(post);
        });
        return posts;
    }

    public ForumPost Create(ForumPost post)
    {
        long newId = PostgresQuery.Create(@"
            INSERT INTO forum_posts (content, posted_at, forum_id, author_id)
            VALUES (@content, NOW(), @forum_id, @author_id)
            RETURNING id"
        )
        .ExtendParameters(_mapper.MapToQueryParameters(post))
        .ExecuteScalar<long>(v => Convert.ToInt64(v));

        // Vraćam post ponovo iz baze da bi upvote/downvote i posted_at bili ispravni
        return GetById(newId)
            ?? throw new InvalidOperationException(
                $"Komentar sa ID-om {newId} nije pronađen nakon INSERT-a.");
    }

    private ForumPost? GetById(long postId)
    {
        var query = PostgresQuery.Create($@"
            SELECT {PostSelectColumns}
            FROM forum_posts fp
            {PostJoins}
            WHERE fp.id = @post_id
            {PostGroupBy}"
        ).AddParameter("post_id", postId);

        ForumPost? result = null;
        query.ExecuteReader(reader => result = _mapper.MapToEntity(reader));
        return result;
    }
}