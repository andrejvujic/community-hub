namespace CommunityHub.Application.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    public User? Get(long id);
    public long? GetIdByCredentials(string username, string password);
    public Role? GetRoleById(long id);
}