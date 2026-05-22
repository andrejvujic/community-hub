using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdInfoPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly AdService _adService;

        public AdDto Ad { get; }

        public MyAdInfoPage(AdDto ad)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            InitializeComponent();
            DataContext = this;

            Ad = ad;
        }

        private void ArchiveAdButton_Click(object sender, RoutedEventArgs e)
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
