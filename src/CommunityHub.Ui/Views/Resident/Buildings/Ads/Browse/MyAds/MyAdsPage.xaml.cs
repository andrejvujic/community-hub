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

        private void CreateAdButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.OpenCreateAdPage();

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdDto ad) return;

            _viewModel.OpenAdDetailsPage(ad);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
