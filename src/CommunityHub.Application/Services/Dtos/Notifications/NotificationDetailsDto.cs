using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityHub.Application.Services.Dtos.Notifications
{
    public abstract class NotificationDetailsDto
    {
        public long Id { get; protected set; }
        public long NotificationId { get; protected set; }

        public NotificationDetailsDto(long id, long notificationId) {
            Id = id;
            NotificationId = notificationId;
        }
    }
}
