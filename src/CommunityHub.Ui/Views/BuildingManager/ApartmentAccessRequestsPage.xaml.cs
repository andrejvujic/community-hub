using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentAccessRequestsPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        private readonly ApartmentAccessRequestRepository _apartmentAccessRequestRepository = new ApartmentAccessRequestRepository();
        public ObservableCollection<ApartmentAccessRequest> ApartmentAccessRequests { get; set; } = new ObservableCollection<ApartmentAccessRequest>();
        public ApartmentAccessRequestsPage(User user, Building building)
        {
            _user = user;
            _building = building;
            InitializeComponent();
            DataContext = this;
            SetStreetInfo();
            LoadApartmentAccessRequests();
        }

        public void LoadApartmentAccessRequests()
        {
            ApartmentAccessRequests.Clear();
            var requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id);
            foreach (var request in requests)
            {
                ApartmentAccessRequests.Add(request);
            }

            borderChangeStatus.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }

        private void LeftArrow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;
            if (homeWindow != null)
            {
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
            }
        }
        private void SetStreetInfo()
        {
            streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox statusFilter = (sender as ComboBox)!;
            ComboBoxItem selectedItem = (statusFilter.SelectedItem as ComboBoxItem)!;

            string filter = selectedItem.Content.ToString()!;

            ApartmentAccessRequests.Clear();

            IEnumerable<ApartmentAccessRequest> requests = Enumerable.Empty<ApartmentAccessRequest>();

            switch (filter)
            {
                case "Svi":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id);
                    break;

                case "Prihvaćen":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id, status: Status.Accepted);
                    break;

                case "Čeka odobrenje":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id, status: Status.Waiting);
                    break;

                case "Odbijen":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id, status: Status.Rejected);
                    break;
            }

            foreach (var request in requests)
            {
                ApartmentAccessRequests.Add(request);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRow = requestsDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                borderChangeStatus.BorderBrush = new SolidColorBrush(Colors.Black);
                gridChangeStatus.IsEnabled = true;
            }
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni da želite otkazati promenu statusa zahteva?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ResetAll();
            }
        }

        private void ResetAll()
        {
            statusComboBox.SelectedIndex = 0;
            commentTextBox.Text = string.Empty;
            requestsDataGrid.UnselectAll();
            gridChangeStatus.IsEnabled = false;
            borderChangeStatus.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (statusComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Molimo odaberite status zahteva.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Da li ste sigurni da želite promeniti status zahteva?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedRequest = requestsDataGrid.SelectedItem as ApartmentAccessRequest;
                if (selectedRequest != null)
                {
                    Status newStatus = statusComboBox.SelectedIndex == 1 ? Status.Accepted : Status.Rejected;
                    string comment = commentTextBox.Text;
                    int rowsAffected = _apartmentAccessRequestRepository.UpdateStatus(selectedRequest.Id, newStatus, comment);

                    if (rowsAffected == 1)
                    {
                        LoadApartmentAccessRequests();
                        MessageBox.Show("Status zahteva je uspešno promenjen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetAll(); 
                    }
                }
            }
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(commentTextBox == null) return;
            

            if (statusComboBox.SelectedIndex == 2)
            {
                commentTextBox.IsEnabled = true;
            }
            else
            {
                commentTextBox.IsEnabled = false;
                commentTextBox.Text = string.Empty;
            }
        }
    }
}