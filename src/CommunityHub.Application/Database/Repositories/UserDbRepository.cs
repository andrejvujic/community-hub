using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class UserDbRepository
{
    private readonly UserMapper _userMapper = new UserMapper();
    
    public long? GetIdByCredentials(string username, string password)
    {
        var query = PostgresQuery.Create(
            "SELECT id FROM users WHERE username = @username AND password = @password"
        )
        .AddParameter("username", username)
        .AddParameter("password", password);
        return query.ExecuteScalar<long?>(value => Convert.ToInt64(value));
    }
    public Role? GetRoleById(long id)
    {
        var query = PostgresQuery.Create(
            "SELECT user_role FROM users WHERE id = @id"
        )
        .AddParameter("id", id);
        return query.ExecuteScalar<Role?>(value => (Role)value!);
    }
    public User? GetUserById(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT id as user_id, username, password, name, surname, user_role " +
            "FROM users WHERE id = @id"
            ).AddParameter("id", id);

        User? user = null;
        query.ExecuteReader(reader => user = _userMapper.MapToEntity(reader));

        return user;
    }
}