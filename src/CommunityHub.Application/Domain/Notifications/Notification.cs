using CommunityHub.Application.Domain.Notifications.Details;

namespace CommunityHub.Application.Domain.Notifications
{
    public class Notification
    {
        public long Id { get; }

        public long RecipientId { get; }
        public User? Recipient { get; }

        public string Title { get; } = string.Empty;
        public string Message { get; } = string.Empty;

        public DateTime SentAt { get; }

        public NotificationType Type { get; }
        public NotificationStatus Status { get; }
        public bool IsRead { get => Status == NotificationStatus.Read; }

        public NotificationDetails? Details { get; }
        public bool HasDetails { get => Details is not null; }

        public Notification(
            long recipientId,
            string title,
            string message,
            DateTime sentAt,
            NotificationType type,
            User? recipient = null,
            NotificationStatus status = NotificationStatus.Unread,
            NotificationDetails? details = null,
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

        public static Notification Create(
            long recipientId,
            string title,
            string message,
            NotificationType type,
            NotificationDetails? details = null)
        {
            return new Notification(
                recipientId,
                title,
                message,
                DateTime.Now,
                type,
                details: details
            );
        }
    }
}
