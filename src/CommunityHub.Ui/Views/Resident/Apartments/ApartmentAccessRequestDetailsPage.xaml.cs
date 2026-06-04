using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Apartments
{
    public partial class ApartmentAccessRequestDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly ApartmentAccessRequestService _apartmentAccessService = new();

        public ApartmentAccessRequestDto Request { get; }

        public ApartmentAccessRequestDetailsPage(ApartmentAccessRequestDto request)
        {
            InitializeComponent();
            DataContext = this;

            Request = request;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this is IAlertServiceConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Upozorenje",
                        $"Da li ste sigurni da želite da otkažete zahtjev za pridruživanje zgradi {Request.Apartment!.Building.DisplayAddress}? Ova akcija će rezultovati u trajnom brisanju Vašeg zahtjeva.",
                        AlertType.Warning
                    )
                    .AddOption("Otkaži")
                    .AddOption("Nastavi", () => {
                        _apartmentAccessService.Cancel(Request);
                        Pop();
                    })
                );
        }
    }
}
