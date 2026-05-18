using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Domain.Notifications.RepositoryInterfaces
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
