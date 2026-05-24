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
            _viewModel = new AdEventNotificationDetailsViewModel(userId, notification, this);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
