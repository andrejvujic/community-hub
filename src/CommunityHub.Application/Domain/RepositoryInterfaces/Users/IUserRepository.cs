using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Users;

public interface IUserRepository
{
    public User? Get(long id);
    public long? GetIdByCredentials(string username, string password);
    public Role? GetRoleById(long id);
}