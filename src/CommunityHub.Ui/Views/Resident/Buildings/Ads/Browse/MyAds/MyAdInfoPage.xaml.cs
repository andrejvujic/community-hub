using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdInfoPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly IAdService _adService = Injector.CreateInstance<IAdService>();

        // TODO: Use DTO
        public Ad Ad { get; }

        public MyAdInfoPage(Ad ad)
        {
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
