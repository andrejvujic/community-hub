using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdTimeSlotsPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly long _userId;
        private readonly MyAdTimeSlotsViewModel _viewModel;

        public MyAdTimeSlotsPage(long userId, AdDto ad)
        {
            _viewModel = new MyAdTimeSlotsViewModel(ad);
            _userId = userId;

            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.UpdateReservations();
        }

        private void ReservationItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdReservation reservation) return;

            Push(
                new AdDetailsPage(_userId, reservation.Ad),
                onPop: _ => _viewModel.UpdateReservations()
            );
        }
    }
}
