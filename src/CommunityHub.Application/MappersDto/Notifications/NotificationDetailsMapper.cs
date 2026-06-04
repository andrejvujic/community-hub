using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Services.Dtos.Notifications;

namespace CommunityHub.Application.MappersDto.Notifications
{
    public class NotificationDetailsMapper : IMapper<NotificationDetails, NotificationDetailsDto>
    {
        IMapper<AdEventNotificationDetails, AdEventNotificationDetailsDto> _adEventNotificationDetailsMapper 
            = new AdEventNotificationDetailsMapper();

        public NotificationDetails MapToEntity(NotificationDetailsDto dto)
        {
            if (dto is AdEventNotificationDetailsDto adEventDetails)
                return _adEventNotificationDetailsMapper.MapToEntity(adEventDetails);

            throw new NotImplementedException($"MapToEntity isn't implemented for type {dto.GetType()}");
        }

        public NotificationDetailsDto MapToDto(NotificationDetails entity)
        {
            if (entity is AdEventNotificationDetails adEventDetails)
                return _adEventNotificationDetailsMapper.MapToDto(adEventDetails);

            throw new NotImplementedException($"MapToDto isn't implemented for type {entity.GetType()}");
        }
    }
}
