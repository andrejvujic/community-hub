using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds
{
    public partial class MatchingAdsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly AdService _adService;
        private readonly AdMatchSubscriptionService _adMatchSubscriptionService;

        public Ad Ad { get; }
        public ObservableCollection<Ad> MatchingAds { get; set; } = new();
        public bool HasMatchingAds => MatchingAds.Count > 0;

        public MatchingAdsPage(long userId, Ad ad)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _adMatchSubscriptionService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdMatchSubscriptionRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>(),
                Injector.CreateInstance<INotificationRepository>()
            );

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
            foreach (var ad in _adService.GetMatchingAds(Ad))
                MatchingAds.Add(ad);
        }

        private void CreateAdMatchSubscription()
            => _adMatchSubscriptionService.Create(
                new AdMatchSubscription(
                    subscribedAd: Ad,
                    subscribedAdId: Ad.Id
                )
            );

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not Ad ad) return;

            Push(
                new AdDetailsPage(_userId, ad),
                onPop: _ => LoadMatchingAds()
            );
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
