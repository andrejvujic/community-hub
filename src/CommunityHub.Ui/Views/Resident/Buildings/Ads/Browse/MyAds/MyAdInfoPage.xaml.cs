using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Buildings.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdInfoPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly AdRepository _adRepository = new();

        public Ad Ad { get; }

        public MyAdInfoPage(Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            Ad = ad;
        }

        private void ArchiveAd()
        {
            Ad.Archive();
            _adRepository.Update(Ad);

            OnPropertyChanged(nameof(Ad));
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
                        callback: () => ArchiveAd()
                    )
                );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
