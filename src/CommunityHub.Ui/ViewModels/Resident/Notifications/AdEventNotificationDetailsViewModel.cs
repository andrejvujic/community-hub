using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;

namespace CommunityHub.Ui.ViewModels.Resident.Notifications
{
    public enum AdAvailability
    {
        Available,
        NotFound,
        Expired
    }

    public class AdEventNotificationDetailsViewModel : BaseViewModel
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
        public RelayCommand OpenAdCommand { get; }

        public AdEventNotificationDetailsViewModel(long userId, NotificationDto notification, MainNavigationConsumerPage page) : base(page)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            UserId = userId;
            Notification = notification;
            Ad = _adService.Get(NotificationDetails.AdId);
            OpenAdCommand = new RelayCommand(OpenAd);
        }

        public AdAvailability GetAdAvailability()
        {
            if (Ad is null)
                return AdAvailability.NotFound;

            if (Ad.IsActive && NotificationDetails.EventType is AdEventType.MatchingAdCreated)
                return AdAvailability.Expired;

            return AdAvailability.Available;
        }

        public void OpenAd()
        {
            var availability = GetAdAvailability();
            if (availability != AdAvailability.Available)
            {
                ShowAdAvailabilityCheckResult(availability);
                return;
            }

            switch (NotificationDetails.EventType)
            {
                case AdEventType.MatchingAdCreated:
                    Push(new AdDetailsPage(UserId, Ad!, AdDetailsPageTab.TimeSlots));
                    break;
                case AdEventType.AdTimeSlotsReserved:
                    Push(new MyAdDetailsPage(UserId, Ad!, MyAdDetailsPageTab.TimeSlots));
                    break;
            }
        }

        private void ShowAdAvailabilityCheckResult(AdAvailability result)
        {
            switch (result)
            {
                case AdAvailability.NotFound:
                    ShowAlert(
                        Alert.Create("Greška", "Traženi oglas nije pronađen.", AlertType.Error)
                            .AddOption("U redu")
                    );
                    break;
                case AdAvailability.Expired:
                    ShowAlert(
                        Alert.Create(
                            "Greška",
                            "Traženi oglas je istekao i više nije dostupan za zakazivanje termina.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }
    }
}
