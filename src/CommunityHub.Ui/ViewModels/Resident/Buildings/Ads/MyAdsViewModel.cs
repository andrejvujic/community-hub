using CommunityHub.Application;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    class MyAdsViewModel : BaseViewModel
    {
        public long UserId { get; }
        public long BuildingId { get; }

        private readonly AdService _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

        public ObservableCollection<AdDto> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;

        public MyAdsViewModel(long userId, long buildingId, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
            BuildingId = buildingId;
        }

        public void OpenCreateAdPage()
            => Push(
                new CreateAdPage(UserId, BuildingId),
                onPop: value =>
                {
                    if (value is not AdDto publishedAd) return;
                    UpdateAds();
                    Push(new MatchingAdsPage(UserId, publishedAd));
                }
            );

        public void OpenAdDetailsPage(AdDto ad)
            => Push(
                new MyAdDetailsPage(UserId, ad),
                onPop: _ => UpdateAds()
            );

        public void UpdateAds()
        {
            Ads.Clear();
            var ads = _adService.GetByAuthorAndBuilding(UserId, BuildingId);
            foreach (var ad in ads)
                Ads.Add(ad);

            OnPropertyChanged(nameof(HasAds));
        }

    }
}
