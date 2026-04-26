using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
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

        private void DeleteRequest()
        {
            _rentalRequestRepository.Delete(Request.Id);
            Pop();
        }

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Upozorenje",
                        content: "Da li ste sigurni da želite da odbijete predloženi opseg datuma? Ovo će rezultovati u trajnom brisanju vašeg zahtjeva.",
                        type: AlertType.Warning
                    )
                    .AddOption("Otkaži")
                    .AddOption("Nastavi", () => DeleteRequest())
                );
        }

        private void AcceptProposedDateRangeChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Request.ProposedStartDate == null || Request.ProposedEndDate == null)
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
                return;
            }

            Request.AcceptProposedDateRange();
            Request = Request.CopyWith(status: RentalRequestStatus.Waiting);
            _rentalRequestRepository.Update(Request);

            OnPropertyChanged(nameof(Request));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
