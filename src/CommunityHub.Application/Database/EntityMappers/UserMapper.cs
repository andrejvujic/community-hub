using CommunityHub.Application.Domain;
using System.Data;
using System.Diagnostics;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class UserMapper : EntityMapper<User>
    {
        public override User MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("user_id", columnPrefix));
            string username = reader.GetString(reader.GetOrdinalPrefixed("username", columnPrefix));
            string password = reader.GetString(reader.GetOrdinalPrefixed("password", columnPrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("name", columnPrefix));
            string surname = reader.GetString(reader.GetOrdinalPrefixed("surname", columnPrefix));
            Role role = (Role)reader.GetValue(reader.GetOrdinalPrefixed("user_role", columnPrefix));
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
