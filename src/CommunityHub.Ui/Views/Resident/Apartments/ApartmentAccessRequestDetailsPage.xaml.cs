using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Apartments
{
    public partial class ApartmentAccessRequestDetailsPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly ApartmentAccessRequestRepository _apartmentAccessRepository;

        public ApartmentAccessRequest Request { get; }

        public ApartmentAccessRequestDetailsPage(ApartmentAccessRequest request)
        {
            InitializeComponent();
            DataContext = this;

            _apartmentAccessRepository = new ApartmentAccessRequestRepository();
            Request = request;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void CancelRequest()
        {
            _apartmentAccessRepository.Delete(Request.Id);
            Pop();
        }

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Request.IsPending) return;

            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Upozorenje",
                        $"Da li ste sigurni da želite da otkažete zahtjev za pridruživanje zgradi {Request.Apartment.Building.Address.Street.Name} {Request.Apartment.Building.Address.StreetNumber}? Ova akcija će rezultovati u trajnom brisanju Vašeg zahtjeva.",
                        AlertType.Warning
                    )
                    .AddOption("Otkaži")
                    .AddOption("Nastavi", () => CancelRequest())
                );
        }
    }
}
