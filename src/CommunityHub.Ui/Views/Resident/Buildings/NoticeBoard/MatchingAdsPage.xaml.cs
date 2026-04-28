using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Ads;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class MatchingAdsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly AdRepository _adRepository = new();
        private readonly AdMatchSubscriptionRepository _adMatchSubscriptionRepository = new();

        public Ad Ad { get; }
        public ObservableCollection<Ad> MatchingAds { get; set; } = new();
        public bool HasMatchingAds => MatchingAds.Count > 0;

        public MatchingAdsPage(long userId, Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;

            LoadMatchingAds();
            if (!HasMatchingAds)
                CreateAdMatchSubscription();
        }

        private void LoadMatchingAds()
        {
            MatchingAds.Clear();
            foreach (var ad in _adRepository.GetMatchingAds(Ad.Id))
                MatchingAds.Add(ad);
        }

        private void CreateAdMatchSubscription()
            => _adMatchSubscriptionRepository.Create(
                new AdMatchSubscription(
                    subscribedAd: Ad,
                    subscribedAdId: Ad.Id
                )
            );

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not Ad ad) return;

            Push(new AdDetailsPage(_userId, ad));
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
