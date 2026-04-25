using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications.Details
{
    public partial class AdReservationNotificationDetailsPage : MainNavigatorConsumerPage
    {
        private long _userId;
        private readonly AdRepository _adRepository = new();
        public Notification Notification { get; set; }

        public AdReservationNotificationDetailsPage(long userId, Notification notification)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Notification = notification;
        }

        private void OpenAdButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Notification.Details is not AdReservationNotificationDetails details)
                throw new Exception("Notification.Details must be instance of AdReservationNotificationDetails");

            var ad = _adRepository.GetById(details.AdId);
            Debug.WriteLine(ad is null);

            if (ad == null) return;

            Push(
                new PersonalAdDetailsPage(
                    _userId,
                    ad
                )
            );
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
