using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdsPage : MainNavigationConsumerPage
    {
        private readonly MyAdsViewModel _viewModel;
        public MyAdsPage(long userId, long buildingId)
        { 
            InitializeComponent();
            _viewModel = new MyAdsViewModel(userId, buildingId, this);
            DataContext = _viewModel;

            _viewModel.UpdateAds();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
