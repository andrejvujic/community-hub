using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;

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
    }
}
