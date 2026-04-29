using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Domain.Buildings;
using System.Collections.ObjectModel;
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
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository = new ApartmentMembershipRepository();
        public ObservableCollection<ApartmentAccessRequest> ApartmentAccessRequests { get; set; } = new ObservableCollection<ApartmentAccessRequest>();

        public ApartmentAccessRequestsPage(User user, Building building)
        {
            _user = user;
            _building = building;
            InitializeComponent();
            DataContext = this;
            SetStreetInfo();
            RefreshApartmentAccessRequests();
        }

        public void RefreshApartmentAccessRequests()
        {
            ApartmentAccessRequests.Clear();
            var requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id);

            foreach (var request in requests)
                ApartmentAccessRequests.Add(request);

            changeStatusBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }

        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApartmentAccessRequests.Clear();
            ComboBoxItem selectedItem = (statusComboBox.SelectedItem as ComboBoxItem)!;
            string filter = selectedItem.Content.ToString()!;

            Status? status = filter switch
            {
                "Prihvaćen" => Status.Accepted,
                "Čeka odobrenje" => Status.Waiting,
                "Odbijen" => Status.Rejected,
                _ => null
            };

            var requests = _apartmentAccessRequestRepository.GetByUserAndBuilding(_user.Id, _building.Id, status: status);

            foreach (var request in requests)
                ApartmentAccessRequests.Add(request);
        }
        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRow = requestsDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                changeStatusBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                changeStatusGrid.IsEnabled = true;
                commentTextBox.Text = string.Empty;
                changeStatusComboBox.SelectedIndex = 0;
            }
        }
        private void ChangeStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (commentTextBox == null) return;

            var selectedItem = (changeStatusComboBox.SelectedItem as ComboBoxItem)!;
            bool isRejected = selectedItem.Content.ToString() == "Odbijen";

            if (isRejected)
            {
                commentTextBox.IsEnabled = true;
            }
            else
            {
                commentTextBox.IsEnabled = false;
                commentTextBox.Text = string.Empty;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni da želite otkazati promenu statusa zahteva?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                ResetSelectionAndForm();
        }
        private void ResetSelectionAndForm()
        {
            changeStatusComboBox.SelectedIndex = 0;
            statusComboBox.SelectedIndex = 0;
            commentTextBox.Text = string.Empty;
            requestsDataGrid.UnselectAll();
            changeStatusGrid.IsEnabled = false;
            changeStatusBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (changeStatusComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Molimo odaberite status zahteva.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Da li ste sigurni da želite promeniti status zahteva?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var request = requestsDataGrid.SelectedItem as ApartmentAccessRequest;
            if (request == null) return;

            Status newStatus = GetSelectedStatus();
            bool successUpdate = UpdateRequestStatus(request, newStatus);

            if (newStatus == Status.Accepted && successUpdate)
                CreateApartmentMembership(request);
        }

        private Status GetSelectedStatus()
        {
            return changeStatusComboBox.SelectedIndex == 1 ? Status.Accepted : Status.Rejected;
        }
        private bool UpdateRequestStatus(ApartmentAccessRequest request, Status newStatus)
        {
            int rowsAffected = _apartmentAccessRequestRepository.UpdateStatus(request.Id, newStatus, commentTextBox.Text);
            if (rowsAffected == 0) return false;

            RefreshApartmentAccessRequests();
            MessageBox.Show("Status zahteva je uspešno promenjen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetSelectionAndForm();
            return true;
        }
        private void CreateApartmentMembership(ApartmentAccessRequest request)
        {
            ApartmentMembership membership = new ApartmentMembership(
                resident: request.Applicant,
                apartment: request.Apartment,
                acceptanceDate: DateTime.Now
            );

            _apartmentMembershipRepository.Create(membership);
        }
    }
}