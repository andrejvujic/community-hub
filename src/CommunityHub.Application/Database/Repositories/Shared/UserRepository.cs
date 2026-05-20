using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Repositories.Shared;

public class UserRepository : IUserRepository
{
    private readonly UserMapper _userMapper = new();

    public User? Get(long id)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
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

        connection.ExecuteReader(
            query,
            reader => {
                user = _userMapper.MapToEntity(reader);
            }
        );

        return user;
    }

    public long? GetIdByCredentials(string username, string password)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
                "SELECT id FROM users WHERE username = @username AND password = @password"
            )
            .AddParameter("username", username)
            .AddParameter("password", password);

        return connection.ExecuteScalar<long?>(
            query,
            value => Convert.ToInt64(value)
        );
    }

    public Role? GetRoleById(long id)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create("SELECT user_role FROM users WHERE id = @id")
            .AddParameter("id", id);

        return connection.ExecuteScalar<Role?>(
            query,
            value => value is Role role ? role : null
        );
    }
}