using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Modules.Buildings.Core.Services.Apartments;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Apartments
{
    public partial class RequestApartmentAccessPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly RequestApartmentAccessViewModel _viewModel;

        public RequestApartmentAccessPage(long userId, Building building)
        {
            InitializeComponent();
            _viewModel = new RequestApartmentAccessViewModel(userId, building);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
        private void CreateAccessRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _viewModel.Submit();
            ShowValidationResult(result);
        }

        private void ShowValidationResult(AccessRequestValidationResult result)
        {
            if (this is not IAlertConsumer consumer) return;

            switch (result)
            {
                case AccessRequestValidationResult.ApartmentNotFound:
                    consumer.ShowAlert(
                        Alert.Create(
                            "Greška",
                            $"Stan broj {_viewModel.ApartmentNumber} ne postoji u zgradi {_viewModel.Building.DisplayAddress}. Provjerite uneseni broj stana i pokušajte ponovo.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;

                case AccessRequestValidationResult.ApartmentOccupied:
                        consumer.ShowAlert(
                            Alert.Create(
                                "Upozorenje",
                                $"Da li ste sigurni da ste unijeli ispravan broj stana? Već postoji član koji je pridružen stanu broj {_viewModel.ApartmentNumber} u zgradi {_viewModel.Building.DisplayAddress}.",
                                AlertType.Warning
                            )
                            .AddOption("Otkaži")
                            .AddOption("Nastavi", () => _viewModel.Submit())
                        );
                    break;

                case AccessRequestValidationResult.AlreadyMember:
                    consumer.ShowAlert(
                          Alert.Create(
                              "Greška",
                              $"Nije moguće kreirati zahtjev za pridruženje stanu broj {_viewModel.ApartmentNumber} u zgradi {_viewModel.Building.DisplayAddress} zato što ste već učlanjeni u ovaj stan.",
                              AlertType.Error
                          )
                          .AddOption("U redu")
                      );
                    break;

                case AccessRequestValidationResult.PendingRequestExists:
                    consumer.ShowAlert(
                          Alert.Create(
                              "Greška",
                              $"Nije moguće kreirati zahtjev za pridruženje stanu broj {_viewModel.ApartmentNumber} u zgradi {_viewModel.Building.DisplayAddress} zato što već imate zahtjev za ovaj stan koji je još uvijek u fazi obrade.",
                              AlertType.Error
                          )
                          .AddOption("U redu")
                      );
                    break;
            }
        }
    }
}
