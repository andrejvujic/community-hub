using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public class UserMapper : Mapper<User>
    {
        private UserMapper() { }
        public static UserMapper Instance => new UserMapper();

        public override User ToModel(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("user_id"));
            string username = reader.GetString(reader.GetOrdinal("username"));
            string password = reader.GetString(reader.GetOrdinal("password"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string surname = reader.GetString(reader.GetOrdinal("surname"));
            Role role = (Role)reader.GetValue(reader.GetOrdinal("user_role"));
            return new User(id, username, password, name, surname, role);
        }
    }
}
