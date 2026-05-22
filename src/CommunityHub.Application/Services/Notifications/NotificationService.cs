using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.MappersDto.Notifications;
using CommunityHub.Application.Services.Dtos.Notifications;

namespace CommunityHub.Application.Services.Notifications
{
    public class NotificationService: BaseService<Notification, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) 
            : base(mapper: new NotificationMapper())
        {
            _notificationRepository = notificationRepository;
        }

        public void Create(NotificationDto notification)
            => _notificationRepository.Create(
                MapToEntity(notification)    
            );

        public List<NotificationDto> GetNotificationsForUser(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        ) => MapToDto(
                _notificationRepository.GetByUserId(
                    userId,
                    order: order,
                    status: status
                )
            );

        public int GetNotificationCountForUser(long userId, NotificationStatus? status = null)
        {
            var notifications = _notificationRepository.GetByUserId(
                userId,
                status: status
            );
            return notifications.Count;
        }

        public NotificationDto MarkAsRead(NotificationDto notification)
        {
            var targetNotification = MapToEntity(notification);

            targetNotification.MarkAsRead();
            _notificationRepository.Update(targetNotification);

            return MapToDto(targetNotification);
        }
    }
}
