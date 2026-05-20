using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds
{
    public partial class AdsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly long _buildingId;

        private readonly AdService _adService; 

        public ObservableCollection<Ad> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;

        public AdsPage(long userId, long buildingId)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;

            UpdateAds();
        }

        private void UpdateAds()
        {
            var ads = _adService.GetByBuilding(
                _buildingId,
                status: AdStatus.Active
            );

            Ads.Clear();
            foreach (var ad in ads)
                Ads.Add(ad);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void CreateAdButton_Click(object sender, RoutedEventArgs e)
        {
            Push(
                new CreateAdPage(
                    userId: _userId,
                    buildingId: _buildingId
                ),
                onPop: (object? value) =>
                {
                    if (value is Ad publishedAd)
                    {
                        UpdateAds();
                        Push(new MatchingAdsPage(_userId, publishedAd));
                        return;
                    }
                }
             );
        }

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not Ad ad) return;

            if (ad.Author.Id == _userId)
            {
                Push(new MyAdDetailsPage(_userId, ad));
                return;
            }

            Push(new AdDetailsPage(_userId, ad));
        }
    }
}
