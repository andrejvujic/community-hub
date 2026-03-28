using System.Data;
using System.Diagnostics;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class UserDbRepository
{
    public long? GetIdByCredentials(string username, string password)
    {
        // using blok automatski zatvara konekciju na kraju bloka u kom je pozvan
        using IDbConnection connection = PostgresConnection.CreateConnection();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT id FROM users WHERE username = @username AND password = @password";

        // Parametrizovani upiti sprečavaju SQL injection napade
        IDbDataParameter usernameParam = command.CreateParameter();
        usernameParam.ParameterName = "@username";
        usernameParam.Value = username;
        command.Parameters.Add(usernameParam);

        IDbDataParameter passwordParam = command.CreateParameter();
        passwordParam.ParameterName = "@password";
        passwordParam.Value = password;
        command.Parameters.Add(passwordParam);

        // ExecuteScalar vraća prvu kolonu prvog reda (id u ovom slučaju)
        object? result = command.ExecuteScalar();

        if (result != null)
        {
            return Convert.ToInt64(result);
        }

        return null;
    }

    public Role? GetRoleById(long id)
    {
        using IDbConnection connection = PostgresConnection.CreateConnection();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT user_role FROM users WHERE id = @id";

        IDbDataParameter idParam = command.CreateParameter();
        idParam.ParameterName = "@id";
        idParam.Value = id;
        command.Parameters.Add(idParam);

        object? result = command.ExecuteScalar();

        if (result == null) return null;

        Debug.WriteLine(result);

        return null;
    }
}
