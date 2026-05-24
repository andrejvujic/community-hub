using CommunityHub.Application;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    class MyAdsViewModel : INotifyPropertyChanged
    {
        public long UserId { get; }
        public long BuildingId { get; }

        private readonly AdService _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

        public ObservableCollection<AdDto> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0;

        public MyAdsViewModel(long userId, long buildingId)
        {
            UserId = userId;
            BuildingId = buildingId;
        }

        public void UpdateAds()
        {
            Ads.Clear();
            var ads = _adService.GetByAuthorAndBuilding(UserId, BuildingId);
            foreach (var ad in ads)
                Ads.Add(ad);

            OnPropertyChanged(nameof(HasAds));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
