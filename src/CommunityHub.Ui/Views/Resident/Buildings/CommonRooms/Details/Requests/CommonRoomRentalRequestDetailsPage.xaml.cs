using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly CommonRoomRentalRequestDetailsViewModel _viewModel;
        public CommonRoomRentalRequestDetailsPage(CommonRoomRentalRequestDto request)
        {
            InitializeComponent();
            _viewModel = new CommonRoomRentalRequestDetailsViewModel(request);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this is IAlertServiceConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Upozorenje",
                        content: "Da li ste sigurni da želite da odbijete predloženi opseg datuma? Ovo će rezultovati u trajnom brisanju vašeg zahtjeva.",
                        type: AlertType.Warning
                    )
                    .AddOption("Otkaži")
                    .AddOption("Nastavi",
                        () => {
                            _viewModel.CancelRequest();
                            Pop();
                        }
                    )
                );
        }

        private void AcceptProposedDateRangeButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AcceptProposedRequestDateRange();

            if (this is IAlertServiceConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        title: "Informacija",
                        content: "Uspješno ste prihvatili predloženi opseg datuma. Vaš zahtjev je ponovo u fazi obrade i biće pregledan od strane upravnika.",
                        type: AlertType.Info
                    )
                    .AddOption("U redu")
                );
        }
    }
}
