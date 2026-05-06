using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications.Details
{
    enum AdAvailability
    {
        Available,
        NotFound,
        Expired
    }

    public partial class AdEventNotificationDetailsPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private long _userId;
        private readonly IAdService _adService = Injector.CreateInstance<IAdService>();
        public Notification Notification { get; set; }

        public AdEventNotificationDetailsPage(long userId, Notification notification)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Notification = notification;
        }

        private void ShowAdAvailabilityCheckResult(AdAvailability result)
        {
            if (this is not IAlertConsumer consumer) return;

            switch (result) {
                case AdAvailability.NotFound:
                    consumer.ShowAlert(
                        Alert.Create(
                            "Greška",
                            "Traženi oglas nije pronađen.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
                case AdAvailability.Expired:
                    consumer.ShowAlert(
                        Alert.Create(
                            "Greška",
                            "Traženi oglas je istekao i više nije dostupan za zakazivanje termina..",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }

        private AdAvailability CheckAdAvailability(Ad? ad, AdEventType eventType)
        {
            if (ad == null)
                return AdAvailability.NotFound;

            if (!ad.IsActive && eventType == AdEventType.MatchingAdCreated)
                return AdAvailability.Expired;

            return AdAvailability.Available;
        }

        private void OpenAdButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Notification.Details is not AdEventNotificationDetails details)
                throw new Exception("Notification.Details must be an instance of AdEventNotificationDetails");

            var ad = _adService.Get(details.AdId);
            var availability = CheckAdAvailability(ad, details.EventType);

            if (availability == AdAvailability.Available)
            {
                switch (details.EventType)
                {
                    case AdEventType.MatchingAdCreated:
                        OpenMatchingAdTimeSlotsPage(ad!);
                        break;
                    case AdEventType.AdTimeSlotsReserved:
                        OpenPersonalAdTimeSlotsPage(ad!);
                        break;
                }
                return;
            }

            ShowAdAvailabilityCheckResult(availability);
        }

        private void OpenMatchingAdTimeSlotsPage(Ad ad)
            => Push(
                new AdDetailsPage(
                    _userId,
                    ad,
                    initialTab: AdDetailsPageTab.TimeSlots
                )
            );

        private void OpenPersonalAdTimeSlotsPage(Ad ad)
            => Push(
                new MyAdDetailsPage(
                    _userId,
                    ad,
                    initialTab: MyAdDetailsPageTab.TimeSlots
                )
            );

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
