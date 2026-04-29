using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Buildings.Ads;
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
        private readonly AdRepository _adRepository = new();
        public ObservableCollection<Ad> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;

        public AdsPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;

            UpdateAds();
        }

        private void UpdateAds()
        {
            Ads.Clear();
            List<Ad> ads = _adRepository.GetByBuilding(_buildingId, AdStatus.Active);
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
