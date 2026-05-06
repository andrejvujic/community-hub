using CommunityHub.Application.Modules.Shared.API.Public.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Shared.Core.Services.Notifications
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void Create(Notification notification)
        {
            _notificationRepository.Create(notification);
        }
    }
}
