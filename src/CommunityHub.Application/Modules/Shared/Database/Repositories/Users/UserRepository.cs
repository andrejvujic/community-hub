using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Shared.Database.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly UserMapper _userMapper = new();
    public User? Get(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                id as user_id,
                username,
                password,
                name,
                surname,
                user_role
            FROM users
            WHERE id = @id"
        )
        .AddParameter("id", id);

        User? user = null;
        query.ExecuteReader(
            reader => user = _userMapper.MapToEntity(reader)
        );
        return user;
    }

    public long? GetIdByCredentials(string username, string password)
    {
        var query = PostgresQuery.Create(
            "SELECT id FROM users WHERE username = @username AND password = @password"
        )
        .AddParameter("username", username)
        .AddParameter("password", password);

        return query.ExecuteScalar<long?>(
            value => Convert.ToInt64(value)
        );
    }

    public Role? GetRoleById(long id)
    {
        var query = PostgresQuery.Create(
            "SELECT user_role FROM users WHERE id = @id"
        )
        .AddParameter("id", id);

        return query.ExecuteScalar<Role?>(
            value => value is Role role ? role : null
        );
    }
}