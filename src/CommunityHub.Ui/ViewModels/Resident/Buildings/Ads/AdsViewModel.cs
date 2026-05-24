using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class AdsViewModel : BaseViewModel
    {
        private readonly long _userId;
        private readonly long _buildingId;
        private readonly AdService _adService;

        public ObservableCollection<AdDto> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;
        public RelayCommand OpenCreateAdCommand { get; }
        public RelayCommand<AdDto> OpenAdDetailsCommand { get; }

        public AdsViewModel(long userId, long buildingId, IMainNavigationConsumer navigationConsumer)
            : base(navigationConsumer)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _userId = userId;
            _buildingId = buildingId;

            OpenCreateAdCommand = new(OpenCreateAdPage);
            OpenAdDetailsCommand = new(OpenAdDetailsPage);

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

        private void OpenCreateAdPage()
            => Push(
                new CreateAdPage(_userId, _buildingId),
                onPop: value =>
                {
                    if (value is not AdDto publishedAd) return;
                    UpdateAds();
                    Push(new MatchingAdsPage(_userId, publishedAd));
                }
            );

        private void OpenAdDetailsPage(AdDto? ad)
        {
            if (ad is null) return;

            if (ad.AuthorId == _userId)
            {
                Push(new MyAdDetailsPage(_userId, ad));
                return;
            }

            Push(new AdDetailsPage(_userId, ad));
        }
    }
}
