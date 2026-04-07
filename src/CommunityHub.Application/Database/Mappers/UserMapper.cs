using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public class UserMapper : DatabaseEntityMapper<User>
    {
        public override User MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("user_id"));
            string username = reader.GetString(reader.GetOrdinal("username"));
            string password = reader.GetString(reader.GetOrdinal("password"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string surname = reader.GetString(reader.GetOrdinal("surname"));
            Role role = (Role)reader.GetValue(reader.GetOrdinal("user_role"));
            return new User(id, username, password, name, surname, role);
        }

        public override Dictionary<string, object?> MapToQueryParameters(User entity)
        {
            return new Dictionary<string, object?>
            {
                { "username", entity.Username },
                { "password", entity.Password },
                { "name", entity.Name },
                { "surname", entity.Surname },
                { "user_role", entity.Role }
            };
        }
    }
}
