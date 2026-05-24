using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds
{
    public partial class AdTimeSlotsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly AdTimeSlotsViewModel _viewModel;

        public AdTimeSlotsPage(long userId, AdDto ad)
        {
            InitializeComponent();
            _viewModel = new AdTimeSlotsViewModel(
                userId: userId,
                ad: ad,
                page: this
            );
            DataContext = _viewModel;



            _viewModel.UpdateReservableTimeSlots();
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.Reserve();

        private void TimeSlotItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdTimeSlotDto timeSlot || timeSlot.IsReserved) return;

            if (_viewModel.IsTimeSlotSelected(timeSlot))
                _viewModel.DeselectTimeSlot(timeSlot);
            else
                _viewModel.SelectTimeSlot(timeSlot);
        }
    }
}
