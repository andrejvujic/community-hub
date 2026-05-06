using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Modules.Shared.API.Public.Users
{
    public interface IUserService
    {
        public User? Get(long id);
    }
}
