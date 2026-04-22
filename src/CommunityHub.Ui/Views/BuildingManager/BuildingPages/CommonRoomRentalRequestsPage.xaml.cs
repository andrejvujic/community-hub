using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomRentalRequestsPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        private Border? _selectedCard;
        private readonly CommonRoomRentalRequestRepository _rentalRequestRepository = new CommonRoomRentalRequestRepository();
        public ObservableCollection<CommonRoomRentalRequest> RentalRequests { get; set; } = [];
        public CommonRoomRentalRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            SetStreetInfo();
            DataContext = this;
        }
        private void CommonRoomRentalRequestsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshRentalRequests();
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private void RefreshRentalRequests()
        {
            RentalRequests.Clear();
            var requests = _rentalRequestRepository.GetAllByBuildingId(_building.Id);

            foreach (var request in requests)
                RentalRequests.Add(request);
        }
        private void RentalRequestCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetCardBackground(sender);

            var border = sender as Border;
            var rentalRequest = border?.DataContext as CommonRoomRentalRequest;
        }
        private void SetCardBackground(object sender)
        {
            if (_selectedCard != null)
                _selectedCard.Background = new SolidColorBrush(Colors.White);

            _selectedCard = sender as Border;
            if (_selectedCard != null)
                _selectedCard.Background = new SolidColorBrush(Colors.LightGray);
        }
    }
}
