using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdTimeSlotsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly MyAdTimeSlotsViewModel _viewModel;

        public MyAdTimeSlotsPage(long userId, AdDto ad)
        {
            _viewModel = new MyAdTimeSlotsViewModel(userId, ad, this);

            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.UpdateReservations();
        }
    }
}
