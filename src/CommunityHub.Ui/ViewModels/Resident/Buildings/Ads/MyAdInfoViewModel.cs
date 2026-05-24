using CommunityHub.Application;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class MyAdInfoViewModel : BaseViewModel
    {
        private readonly AdService _adService;

        public AdDto Ad { get; }
        public RelayCommand ArchiveAdCommand { get; }

        public MyAdInfoViewModel(AdDto ad, IMainNavigationConsumer navigationConsumer) : base(navigationConsumer)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            Ad = ad;
            ArchiveAdCommand = new(ArchiveAd);
        }

        private void ArchiveAd()
            => ShowAlert(
                Alert.Create(
                    "Upozorenje",
                    "Da li ste sigurni da želite da arhivirate oglas? Vaš oglas više neće biti vidljiv ostalim stanarima na oglasnoj tabli. Rezervisani termini neće biti izgubljeni.",
                    AlertType.Warning
                )
                .AddOption("Otkaži")
                .AddOption(
                    "Nastavi",
                    callback: () => {
                        _adService.Archive(Ad);
                        OnPropertyChanged(nameof(Ad));
                    }
                )
            );
    }
}
