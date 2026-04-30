using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestDetailsPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        public CommonRoomRentalRequest Request { get; set; }

        private readonly CommonRoomRentalRequestRepository _rentalRequestRepository = new();

        public CommonRoomRentalRequestDetailsPage(CommonRoomRentalRequest request)
        {
            Request = request;

            InitializeComponent();
            DataContext = this;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void CancelRequest()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Upozorenje",
                        content: "Da li ste sigurni da želite da odbijete predloženi opseg datuma? Ovo će rezultovati u trajnom brisanju vašeg zahtjeva.",
                        type: AlertType.Warning
                    )
                    .AddOption("Otkaži")
                    .AddOption("Nastavi",
                        () => {
                                _rentalRequestRepository.Delete(Request.Id);
                                Pop();
                            }
                        )
                );
        }

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
            => CancelRequest();

        private void AcceptProposedDateRangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Request.HasProposedDateRange)
            {
                ShowErrorAlert();
                return;
            }

            Request.AcceptProposedDateRange();
            Request = Request.CopyWith(status: RentalRequestStatus.Waiting);
            _rentalRequestRepository.Update(Request);
            OnPropertyChanged(nameof(Request));

            ShowSuccessAlert();
        }

        private void ShowErrorAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Greška",
                        content: "Nije bilo moguće prihvatiti promjenu opsega datuma jer ga upravnik nije postavio.",
                        type: AlertType.Error
                    )
                    .AddOption("U redu")
                );
        }

        private void ShowSuccessAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Informacija",
                        content: "Uspješno ste prihvatili predloženi opseg datuma. Vaš zahtjev je ponovo u fazi obrade i biće pregledan od strane upravnika.",
                        type: AlertType.Info
                    )
                    .AddOption("U redu")
                );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
