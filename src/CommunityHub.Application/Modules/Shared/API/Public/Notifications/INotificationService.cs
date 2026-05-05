using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;

namespace CommunityHub.Application.Modules.Shared.API.Public.Notifications
{
    public interface INotificationService
    {
        public void Create(Notification notification);
    }
}
