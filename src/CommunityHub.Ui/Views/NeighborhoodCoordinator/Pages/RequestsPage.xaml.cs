using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository;
        private readonly NeighborhoodMembershipRepository _neighborhoodMembershipRepository;
        private readonly CitizenRepository _citizenRepository;
        public RequestsPage()
        {
            InitializeComponent();
            _neighborhoodAccessRequestRepository = new NeighborhoodAccessRequestRepository();
            _neighborhoodMembershipRepository = new NeighborhoodMembershipRepository();
            _citizenRepository = new CitizenRepository();
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadData();
        }

        private void LoadData()
        {
            Status? selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Tag as Status?;

            var requests = _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(_coordinatorId, selectedStatus);
            RequestsDataGrid.ItemsSource = requests;
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
                var dialog = new RejectDialog(request.Citizen.Name, request.Citizen.Surname, request.Neighborhood.Name);
                dialog.Owner = Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    var updatedRequest = request.Reject(dialog.Comment);
                    _neighborhoodAccessRequestRepository.Update(updatedRequest);

                    LoadData();
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

                LoadData();
                MessageBox.Show($"Korisnik {request.Citizen.Name} {request.Citizen.Surname} je postao novi član kvarta {request.Neighborhood.Name}.", "Prihvaćen zahtev");
            }
        }
    }
}
