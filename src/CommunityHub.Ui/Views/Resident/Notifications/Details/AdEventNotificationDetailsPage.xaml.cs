using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications.Details
{
    enum AdValidationResult
    {
        Valid,
        NotFound
    }

    public partial class AdEventNotificationDetailsPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private long _userId;
        private readonly AdRepository _adRepository = new();
        public Notification Notification { get; set; }

        public AdEventNotificationDetailsPage(long userId, Notification notification)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Notification = notification;
        }

        private void ShowAdValidationResult(AdValidationResult result)
        {
            if (this is not IAlertConsumer consumer) return;

            switch (result) {
                case AdValidationResult.NotFound:
                    consumer.ShowAlert(
                        Alert.Create(
                            "Greška",
                            "Traženi oglas nije pronađen.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }

        private AdValidationResult ValidateAd(Ad? ad)
        {
            if (ad == null)
                return AdValidationResult.NotFound;

            return AdValidationResult.Valid;
        }

        private void OpenAdButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Notification.Details is not AdEventNotificationDetails details)
                throw new Exception("Notification.Details must be an instance of AdEventNotificationDetails");

            var ad = _adRepository.GetById(details.AdId);
            var validationResult = ValidateAd(ad);

            if (validationResult == AdValidationResult.Valid)
            {
                switch (details.EventType)
                {
                    case AdEventType.MatchingAdCreated:
                        OpenAdTimeSlotsPage(ad!);
                        break;
                    case AdEventType.AdTimeSlotsReserved:
                        OpenPersonalAdTimeSlotsPage(ad!);
                        break;
                }
                return;
            }

            ShowAdValidationResult(validationResult);
        }

        private void OpenAdTimeSlotsPage(Ad ad)
            => Push(
                new AdDetailsPage(
                    _userId,
                    ad,
                    initialTab: AdDetailsPageTab.TimeSlots
                )
            );

        private void OpenPersonalAdTimeSlotsPage(Ad ad)
            => Push(
                new PersonalAdDetailsPage(
                    _userId,
                    ad,
                    initialTab: PersonalAdDetailsPageTab.TimeSlots
                )
            );

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
