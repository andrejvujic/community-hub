using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class NoticeBoardPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly long _buildingId;
        private readonly AdRepository _adRepository = new();
        public ObservableCollection<Ad> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;

        public NoticeBoardPage(long userId, long buildingId)
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
            List<Ad> ads = _adRepository.GetAdsForBuilding(_buildingId, AdStatus.Active);
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
                Push(new PersonalAdDetailsPage(_userId, ad));
                return;
            }

            Push(new AdDetailsPage(_userId, ad));
        }
    }
}
