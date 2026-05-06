using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        public void Create(Notification notification);
        public void Update(Notification notification);

        public int GetCountByUserId(long userId, NotificationStatus? status = null);
        public List<Notification> GetByUserId(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        );
    }
}
