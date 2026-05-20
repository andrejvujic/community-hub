using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;

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

        public Notification Notification { get; }
        public AdEventNotificationDetails NotificationDetails
        {
            get
            {
                if (Notification.Details is not AdEventNotificationDetails details)
                    throw new Exception("Notification.Details must be an instance of AdEventNotificationDetails");

                return details;
            }
        }

        public Ad? Ad { get; }

        public AdEventNotificationDetailsViewModel(long userId, Notification notification)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            UserId = userId;
            Notification = notification;
            Ad = LoadAd();
        }

        private Ad? LoadAd() => _adService.Get(NotificationDetails.AdId);

        public AdAvailability GetAdAvailability()
        {
            if (Ad == null)
                return AdAvailability.NotFound;

            if (!Ad.IsActive && NotificationDetails.EventType == AdEventType.MatchingAdCreated)
                return AdAvailability.Expired;

            return AdAvailability.Available;
        }
    }
}
