using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
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
