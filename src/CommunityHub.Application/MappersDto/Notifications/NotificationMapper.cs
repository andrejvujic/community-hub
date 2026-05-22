using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Services.Dtos.Notifications;

namespace CommunityHub.Application.MappersDto.Notifications
{
    public class NotificationMapper : IMapper<Notification, NotificationDto>
    {
        private readonly IMapper<NotificationDetails, NotificationDetailsDto> _detailsMapper
            = new NotificationDetailsMapper();

        public Notification MapToDomain(NotificationDto dto)
        {
            return new Notification(
                recipientId: dto.RecipientId,
                title: dto.Title,
                message: dto.Message,
                sentAt: dto.SentAt,
                type: dto.Type,
                recipient: dto.Recipient,
                status: dto.Status,
                details: dto.Details is not null ? _detailsMapper.MapToDomain(dto.Details) : null,
                id: dto.Id
            );
        }

        public NotificationDto MapToDto(Notification domain)
        {
            return new NotificationDto(
                recipientId: domain.RecipientId,
                title: domain.Title,
                message: domain.Message,
                sentAt: domain.SentAt,
                type: domain.Type,
                recipient: domain.Recipient,
                status: domain.Status,
                details: domain.Details is not null ? _detailsMapper.MapToDto(domain.Details) : null,
                id: domain.Id
            );
        }
    }
}
