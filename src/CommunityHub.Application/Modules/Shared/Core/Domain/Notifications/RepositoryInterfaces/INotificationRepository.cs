using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        public void Create(Notification notification);
        public void CreateMany(List<Notification> notifications);

        public void Update(Notification notification);

        public List<Notification> GetByUserId(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        );
    }
}
