using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class MatchingAdsViewModel : BaseViewModel
    {
        private readonly long _userId;
        private readonly AdService _adService;
        private readonly AdMatchSubscriptionService _adMatchSubscriptionService;

        public AdDto Ad { get; }
        public ObservableCollection<AdDto> MatchingAds { get; } = new();
        public bool HasMatchingAds => MatchingAds.Count > 0;
        public RelayCommand<AdDto> OpenAdDetailsCommand { get; }

        public MatchingAdsViewModel(
            long userId,
            AdDto ad,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
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

            _userId = userId;
            Ad = ad;
            OpenAdDetailsCommand = new(OpenAdDetailsPage);

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
                new AdMatchSubscriptionDto(
                    subscribedAd: Ad,
                    subscribedAdId: Ad.Id
                )
            );

        private void OpenAdDetailsPage(AdDto? ad)
        {
            if (ad is null) return;
            Push(
                new AdDetailsPage(_userId, ad),
                onPop: _ => LoadMatchingAds()
            );
        }
    }
}
