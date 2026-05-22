using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Services.Dtos.Notifications
{
    public class NotificationDto
    {
        public long Id { get; }

        public long RecipientId { get; }
        public User? Recipient { get; }

        public string Title { get; } = string.Empty;
        public string Message { get; } = string.Empty;

        public DateTime SentAt { get; }

        public NotificationType Type { get; }
        public NotificationStatus Status { get; private set; }
        public bool IsRead { get => Status == NotificationStatus.Read; }


        public NotificationDetailsDto? Details { get; }
        public bool HasDetails { get => Details is not null; }

        public NotificationDto(
            long recipientId,
            string title,
            string message,
            DateTime sentAt,
            NotificationType type,
            User? recipient = null,
            NotificationStatus status = NotificationStatus.Unread,
            NotificationDetailsDto? details = null,
            long id = 0
        )
        {
            Id = id;
            RecipientId = recipientId;
            Recipient = recipient;
            Title = title;
            Message = message;
            SentAt = sentAt;
            Type = type;
            Status = status;
            Details = details;
        }
    }
}
