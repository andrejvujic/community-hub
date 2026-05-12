using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Services.Dtos.Users
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Role Role { get; set; }
        public UserDto(long id, string username, string password, string name, string surname, Role role)
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            Role = role;
        }
    }
}