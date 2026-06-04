using CommunityHub.Application;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
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

            _userId = userId;
            Ad = ad;
            OpenAdDetailsCommand = new(OpenAdDetailsPage);

            LoadMatchingAds();
            if (!HasMatchingAds)
                SubscribeToMatchingAds();
        }

        private void LoadMatchingAds()
        {
            MatchingAds.Clear();
            foreach (var ad in _adService.GetMatchingAds(Ad))
                MatchingAds.Add(ad);
        }

        private void SubscribeToMatchingAds()
            => _adService.SubscribeToMatchingAds(Ad);

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
