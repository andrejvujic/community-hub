using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Modules.Buildings.Core.Services.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class RequestCommonRoomRentalPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly RequestCommonRoomRentalViewModel _viewModel;

        public RequestCommonRoomRentalPage(long userId, CommonRoom commonRoom)
        {
            InitializeComponent();
            _viewModel = new RequestCommonRoomRentalViewModel(userId, commonRoom);
            DataContext = _viewModel;
        }

        private void ShowSuccessAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Zahtjev podnesen",
                        "Vaš zahtjev je uspješno podnesen.",
                        AlertType.Info
                    )
                    .AddOption("U redu", () => Pop())
                );
        }

        private void ShowValidationResult(RentalRequestValidationResult result)
        {
            if (this is not IAlertConsumer consumer)
                return;

            switch (result)
            {
                case RentalRequestValidationResult.PendingRequestExists:
                        consumer.ShowAlert(
                              Alert.Create(
                                  "Greška",
                                  "Nije moguće kreirati zahtjev za iznajmljivanje ove zajedničke prostorije zato što ste već imate zahtjev za ovu zajedničku prostoriju koji je još uvijek u fazi obrade.",
                                  AlertType.Error
                              )
                              .AddOption("U redu")
                          );
                    break;
            }
        }

        private void CreateRentalRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _viewModel.ValidateRentalRequest();

            if (result is RentalRequestValidationResult.Valid)
            {
                _viewModel.CreateRentalRequest();
                ShowSuccessAlert();
                return;
            }

            ShowValidationResult(result);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();


        private void ToggleOverlayVisibility(FrameworkElement overlay)
            =>
                overlay.Visibility = overlay.Visibility switch
                {
                    Visibility.Visible => Visibility.Collapsed,
                    Visibility.Collapsed => Visibility.Visible,
                    _ => Visibility.Visible
                };

        private void ToggleSelectDateRangeMenuButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(SelectDateRangeOverlay);

        private void ToggleStartDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(StartDatePicker);

        private void ToggleEndDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(EndDatePicker);

    }
}
