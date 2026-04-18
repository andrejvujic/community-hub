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
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Korisničko ime ne sme biti prazno.", nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Lozinka ne sme biti prazna.", nameof(password));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ime ne sme biti prazno.", nameof(name));

        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Prezime ne sme biti prazno.", nameof(surname));

        Id = id;
        Username = username;
        Password = password;
        Name = name;
        Surname = surname;
        Role = role;
    }
}