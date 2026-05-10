using CommunityHub.Application.Domain.Users;
using System.Data;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class UserMapper : EntityMapper<User>
    {
        public override User? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("user_id")) return null;

            var idColumnIndex = reader.GetOrdinal("user_id");
            if (reader.IsDBNull(idColumnIndex)) return null;

            var id = reader.GetInt64(idColumnIndex);

            var username = reader.GetString(reader.GetOrdinal("username"));
            var password = reader.GetString(reader.GetOrdinal("password"));

            var name = reader.GetString(reader.GetOrdinal("name"));
            var surname = reader.GetString(reader.GetOrdinal("surname"));

            var role = (Role)reader.GetValue(reader.GetOrdinal("user_role"));

            return new User(
                id,
                username,
                password,
                name,
                surname,
                role
            );
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
