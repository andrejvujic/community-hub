using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Neighborhoods;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class RequestsTab : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository = new NeighborhoodAccessRequestRepository();
        private readonly NeighborhoodMembershipRepository _neighborhoodMembershipRepository = new NeighborhoodMembershipRepository();
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        
        public ObservableCollection<NeighborhoodAccessRequest> Requests { get; set; } = new ObservableCollection<NeighborhoodAccessRequest>();

        public RequestsTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadData();
        }

        private void LoadData()
        {
            Status? selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Tag as Status?;

            Requests.Clear();
            var requests = _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(_coordinatorId, selectedStatus);
            foreach (var req in requests)
            {
                Requests.Add(req);
            }
        }

        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is NeighborhoodAccessRequest request)
            {
                bool isWaiting = request.Status == Status.Waiting;
                AcceptButton.IsEnabled = isWaiting;
                RejectButton.IsEnabled = isWaiting;
            }
            else
            {
                AcceptButton.IsEnabled = false;
                RejectButton.IsEnabled = false;
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_neighborhoodAccessRequestRepository != null)
            {
                LoadData();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is NeighborhoodAccessRequest request)
            {
                var dialog = new RejectRequestWindow(request.Citizen.Name, request.Citizen.Surname, request.Neighborhood.Name);
                dialog.Owner = Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    var updatedRequest = request.Reject(dialog.Comment);
                    _neighborhoodAccessRequestRepository.Update(updatedRequest);

                    int index = Requests.IndexOf(request);
                    Requests[index] = updatedRequest;
                    MessageBox.Show($"Korisniku {request.Citizen.Name} {request.Citizen.Surname} je odbijen zahtev za pristup kvartu {request.Neighborhood.Name}.", "Odbijen zahtev");
                }
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is NeighborhoodAccessRequest request)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Da li ste sigurni da želite da prihvatite zahtev korisnika {request.Citizen.Name} {request.Citizen.Surname} za pristup kvartu {request.Neighborhood.Name}?",
                    "Potvrda prihvatanja",
                    MessageBoxButton.YesNo);

                if (result != MessageBoxResult.Yes) return;

                var updatedRequest = request.Accept();
                _neighborhoodAccessRequestRepository.Update(updatedRequest);

                var newMembership = new NeighborhoodMembership(0, DateTime.Now, request.Neighborhood);
                newMembership = _neighborhoodMembershipRepository.Create(newMembership);

                _citizenRepository.UpdateMembership(request.Citizen.Id, newMembership.Id);

                int index = Requests.IndexOf(request);
                Requests[index] = updatedRequest;

                MessageBox.Show($"Korisnik {request.Citizen.Name} {request.Citizen.Surname} je postao novi član kvarta {request.Neighborhood.Name}.", "Prihvaćen zahtev");
            }
        }
    }
}
