using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Database.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuildingFiltered(_user.Id, _building.Id, "accepted");
                    break;

                case "Čeka odobrenje":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuildingFiltered(_user.Id, _building.Id, "waiting");
                    break;

                case "Odbijen":
                    requests = _apartmentAccessRequestRepository.GetByUserAndBuildingFiltered(_user.Id, _building.Id, "rejected");
                    break;
            }

            foreach (var request in requests)
            {
                ApartmentAccessRequests.Add(request);
            }
        }
    }
}
