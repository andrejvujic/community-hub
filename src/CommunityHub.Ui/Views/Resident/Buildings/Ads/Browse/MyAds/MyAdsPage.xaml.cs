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
            _viewModel = new MyAdsViewModel(userId, buildingId);    
            DataContext = _viewModel;

            _viewModel.UpdateAds();
        }

        private void CreateAdButton_Click(object sender, RoutedEventArgs e)
        {
            Push(
                new CreateAdPage(
                    userId: _viewModel.UserId,
                    buildingId: _viewModel.BuildingId
                ),
                onPop: (object? value) =>
                {
                    if (value is AdDto publishedAd)
                    {
                        _viewModel.UpdateAds();
                        Push(new MatchingAdsPage(_viewModel.UserId, publishedAd));
                        return;
                    }
                }
             );
         }

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdDto ad) return;

            Push(
                new MyAdDetailsPage(_viewModel.UserId, ad),
                onPop: _ => _viewModel.UpdateAds()
            );
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
