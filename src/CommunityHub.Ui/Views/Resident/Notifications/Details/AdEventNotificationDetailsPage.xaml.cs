using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Notifications;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications.Details
{
    public partial class AdEventNotificationDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public AdEventNotificationDetailsPage(long userId, NotificationDto notification)
        {
            InitializeComponent();
            DataContext = new AdEventNotificationDetailsViewModel(userId, notification, this);;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
