using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Users.RepositoryInterfaces;

public interface IUserRepository
{
    public User? Get(long id);
    public long? GetIdByCredentials(string username, string password);
    public Role? GetRoleById(long id);
}