using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Notifications;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications.Details
{
    public partial class AdEventNotificationDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly AdEventNotificationDetailsViewModel _viewModel;

        public AdEventNotificationDetailsPage(long userId, NotificationDto notification)
        {
            InitializeComponent();
            _viewModel = new AdEventNotificationDetailsViewModel(userId, notification);
            DataContext = _viewModel;
        }

        private void ShowAdAvailabilityCheckResult(AdAvailability result)
        {
            if (this is not IAlertServiceConsumer consumer) return;

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
                            "Traženi oglas je istekao i više nije dostupan za zakazivanje termina.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }


        private void OpenAdButton_Click(object sender, MouseButtonEventArgs e)
        {
            var availability = _viewModel.GetAdAvailability();

            if (availability == AdAvailability.Available)
            {
                switch (_viewModel.NotificationDetails.EventType)
                {
                    case AdEventType.MatchingAdCreated:
                        OpenMatchingAdTimeSlotsPage();
                        break;
                    case AdEventType.AdTimeSlotsReserved:
                        OpenPersonalAdTimeSlotsPage();
                        break;
                }
                return;
            }

            ShowAdAvailabilityCheckResult(availability);
        }

        private void OpenMatchingAdTimeSlotsPage()
            => Push(
                new AdDetailsPage(
                    _viewModel.UserId,
                    _viewModel.Ad!,
                    initialTab: AdDetailsPageTab.TimeSlots
                )
            );

        private void OpenPersonalAdTimeSlotsPage()
            => Push(
                new MyAdDetailsPage(
                    _viewModel.UserId,
                    _viewModel.Ad!,
                    initialTab: MyAdDetailsPageTab.TimeSlots
                )
            );

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
