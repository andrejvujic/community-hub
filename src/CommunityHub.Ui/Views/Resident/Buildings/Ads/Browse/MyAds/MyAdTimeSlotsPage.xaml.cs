using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdTimeSlotsPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly long _userId;
        private readonly MyAdTimeSlotsViewModel _viewModel;

        public MyAdTimeSlotsPage(long userId, Ad ad)
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
