namespace CommunityHub.Application.Domain;

public class User
{
    public long Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public Role Role { get; private set; }

    public User(long id, string username, string password, string name, string surname, Role role)
    {
        Id = id;
        Username = username;
        Password = password;
        Name = name;
        Surname = surname;
        Role = role;
    }
}