using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class UserMapper : EntityMapper<User>
    {
        public override User MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("user_id", columnNamePrefix));
            string username = reader.GetString(reader.GetOrdinalPrefixed("username", columnNamePrefix));
            string password = reader.GetString(reader.GetOrdinalPrefixed("password", columnNamePrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("name", columnNamePrefix));
            string surname = reader.GetString(reader.GetOrdinalPrefixed("surname", columnNamePrefix));
            Role role = (Role)reader.GetValue(reader.GetOrdinalPrefixed("user_role", columnNamePrefix));
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
