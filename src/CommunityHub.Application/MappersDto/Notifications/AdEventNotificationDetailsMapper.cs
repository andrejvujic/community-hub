using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Services.Dtos.Notifications;

namespace CommunityHub.Application.MappersDto.Notifications
{
    public class AdEventNotificationDetailsMapper : IMapper<AdEventNotificationDetails, AdEventNotificationDetailsDto>
    {
        public AdEventNotificationDetails MapToDomain(AdEventNotificationDetailsDto dto)
            => new AdEventNotificationDetails(
                    adId: dto.AdId,
                    eventType: dto.EventType,   
                    notificationId: dto.NotificationId,
                    id: dto.Id
               );

        public AdEventNotificationDetailsDto MapToDto(AdEventNotificationDetails domain)
            => new AdEventNotificationDetailsDto(
                    adId: domain.AdId,
                    eventType: domain.EventType,
                    notificationId: domain.NotificationId,
                    id: domain.Id
               );
    }
}
