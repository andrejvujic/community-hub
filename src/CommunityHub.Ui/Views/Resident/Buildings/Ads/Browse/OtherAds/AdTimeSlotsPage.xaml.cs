using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds
{
    public partial class AdTimeSlotsPage : MainNavigationConsumerPage, INotifyPropertyChanged, IAlertServiceConsumer
    {
        private readonly AdTimeSlotsViewModel _viewModel;

        public AdTimeSlotsPage(long userId, AdDto ad)
        {
            InitializeComponent();
            _viewModel = new AdTimeSlotsViewModel(
                userId: userId,
                ad: ad
            );
            DataContext = _viewModel;



            _viewModel.UpdateReservableTimeSlots();
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ReserveSelectedTimeSlots();

            /*
            if (this is IAlertServiceConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Greška",
                        $"Nije bilo moguće rezervisati termin {selectedTimeSlot.StartTime:HH.mm} — {selectedTimeSlot.EndTime:HH.mm} na dan {selectedTimeSlot.StartTime.ToString("d. MMMM", _culture)} zato što već imate dogovor sklopljen sa drugim stanarom u ovom terminu.",
                        AlertType.Error
                    ).AddOption("U redu")
                );
            */

            if (this is IAlertServiceConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Rezervacija uspješna",
                        "Uspješno ste rezervisali termine. Autor oglasa je obaviješten o vašoj rezervaciji."
                    ).AddOption("U redu")
                );

            _viewModel.UpdateReservableTimeSlots();
            _viewModel.DeselectAllTimeSlots();
        }

        private void TimeSlotItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdTimeSlotDto timeSlot || timeSlot.IsReserved) return;

            if (_viewModel.IsTimeSlotSelected(timeSlot))
                _viewModel.DeselectTimeSlot(timeSlot);
            else
                _viewModel.SelectTimeSlot(timeSlot);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
