using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class UserDbRepository
{
    public long? GetIdByCredentials(string username, string password)
    {
        var query = Query.Create(
            "SELECT id FROM users WHERE username = @username AND password = @password"
        )
        .AddParameter("username", username)
        .AddParameter("password", password);
        return query.ExecuteScalar<long?>(value => Convert.ToInt64(value));
    }

    public Role? GetRoleById(long id)
    {
        var query = Query.Create(
            "SELECT user_role FROM users WHERE id = @id"
        )
        .AddParameter("id", id);
        return query.ExecuteScalar<Role?>(value => (Role)value!);
    }
}
