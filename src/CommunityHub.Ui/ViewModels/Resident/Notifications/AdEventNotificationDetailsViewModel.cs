using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.Services.Dtos.Notifications;

namespace CommunityHub.Ui.ViewModels.Resident.Notifications
{
    public enum AdAvailability
    {
        Available,
        NotFound,
        Expired
    }

    public class AdEventNotificationDetailsViewModel
    {
        private readonly AdService _adService;

        public long UserId { get; }

        public NotificationDto Notification { get; }
        public AdEventNotificationDetailsDto NotificationDetails
        {
            get
            {
                if (Notification.Details is not AdEventNotificationDetailsDto details)
                    throw new Exception("Notification.Details must be an instance of AdEventNotificationDetails");

                return details;
            }
        }

        public AdDto? Ad { get; }

        public AdEventNotificationDetailsViewModel(long userId, NotificationDto notification)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            UserId = userId;
            Notification = notification;
            Ad = _adService.Get(NotificationDetails.AdId);
        }

        public AdAvailability GetAdAvailability()
        {
            if (Ad is null)
                return AdAvailability.NotFound;

            if (Ad.IsActive && NotificationDetails.EventType is AdEventType.MatchingAdCreated)
                return AdAvailability.Expired;

            return AdAvailability.Available;
        }
    }
}
