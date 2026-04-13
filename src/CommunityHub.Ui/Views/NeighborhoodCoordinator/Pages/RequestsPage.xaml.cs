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
    /// Interaction logic for RequestsView.xaml
    /// </summary>
    public partial class RequestsView : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository;
        private readonly NeighborhoodMembershipRepository _neighborhoodMembershipRepository;
        private readonly CitizenRepository _citizenRepository;
        public RequestsView()
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
            var selectedItem = StatusFilterComboBox.SelectedItem as ComboBoxItem;
            string filterText = selectedItem?.Content.ToString() ?? "Svi";

            Status? selectedStatus = filterText switch
            {
                "Čeka odobrenje" => Status.Waiting,
                "Prihvaćen" => Status.Accepted,
                "Odbijen" => Status.Rejected,
                _ => null
            };

            var requests = _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(_coordinatorId, selectedStatus);

            var displayData = requests.Select(r =>
            {
                string statusText;
                string statusIcon;

                switch (r.Status)
                {
                    case Status.Accepted:
                        statusText = "Prihvaćen";
                        statusIcon = "\uf00c";
                        break;
                    case Status.Rejected:
                        statusText = "Odbijen";
                        statusIcon = "\uf00d";
                        break;
                    default:
                        statusText = "Čeka odobrenje";
                        statusIcon = "\uf017";
                        break;
                }

                return new
                {
                    OriginalRequest = r,
                    Ime = r.Citizen.Name,
                    Prezime = r.Citizen.Surname,
                    Adresa = $"{r.Citizen.Address.Street.Name} {r.Citizen.Address.StreetNumber}, {r.Citizen.Address.City.Name}",
                    Kvart = r.Neighborhood.Name,
                    Datum = r.CreationDate,
                    StatusIcon = statusIcon,
                    StatusText = statusText
                };
            }).ToList();


            RequestsDataGrid.ItemsSource = displayData;
        }

        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                AcceptButton.IsEnabled = false;
                RejectButton.IsEnabled = false;
                return;
            }

            dynamic selected = RequestsDataGrid.SelectedItem;
            NeighborhoodAccessRequest request = selected.OriginalRequest;

            bool isWaiting = request.Status == Status.Waiting;

            AcceptButton.IsEnabled = isWaiting;
            RejectButton.IsEnabled = isWaiting;
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
            dynamic selected = RequestsDataGrid.SelectedItem;
            if (selected == null) return;

            NeighborhoodAccessRequest request = selected.OriginalRequest;

            var dialog = new RejectDialog(selected.Ime, selected.Prezime, selected.Kvart);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                var updatedRequest = request.Reject(dialog.Comment);

                _neighborhoodAccessRequestRepository.Update(updatedRequest);

                LoadData();
                MessageBox.Show($"Korisniku {selected.Ime} {selected.Prezime} je odbijen zahtev za pristup kvartu {selected.Kvart}.", "Odbijen zahtev");
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = RequestsDataGrid.SelectedItem;
            if (selected == null) return;

            MessageBoxResult result = MessageBox.Show(
                $"Da li ste sigurni da želite da prihvatite zahtev korisnika {selected.Ime} {selected.Prezime} za pristup kvartu {selected.Kvart}?",
                "Potvrda prihvatanja",
                MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes) return;

            NeighborhoodAccessRequest request = selected.OriginalRequest;

            var updatedRequest = request.Accept();
            _neighborhoodAccessRequestRepository.Update(updatedRequest);

            var newMembership = new NeighborhoodMembership(0, DateTime.Now, request.Neighborhood);
            newMembership = _neighborhoodMembershipRepository.Create(newMembership);

            _citizenRepository.UpdateMembership(request.Citizen.Id, newMembership.Id);
            

            LoadData();
            MessageBox.Show($"Korisnik {selected.Ime} {selected.Prezime} je postao novi član kvarta {selected.Kvart}.", "Prihvaćen zahtev");
        }
    }
}
