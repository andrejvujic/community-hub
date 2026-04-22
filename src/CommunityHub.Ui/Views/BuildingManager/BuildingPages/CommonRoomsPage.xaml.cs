using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoom;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomsPage : Page
    {
        private readonly CommonRoomRepository _commonRoomRepository = new();
        private readonly CommonRoomRentalRequestRepository _commonRoomRentalRequestRepository = new();
        private readonly Building _building;
        private readonly User _user;
        public ObservableCollection<CommonRoom> CommonRooms { get; set; } = [];
        public CommonRoomsPage(User user, Building building)
        {
            InitializeComponent();
            DataContext = this;
            _user = user;
            _building = building;
            SetStreetInfo();
        }
        private void CommonRoomsPage_Loaded(object sender, RoutedEventArgs e) => RefreshCommonRoomsPage();
        private void RefreshCommonRoomsPage()
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomRepository.GetByUserAndBuilding(_user.Id, _building.Id);

            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);

            floorsUpDown.Maximum = _building.FloorCount;
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li želite da otkažete kreiranje zajedničke prostorije?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            ResetForm();
        }
        private void ResetForm()
        {
            nameTextBox.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
            floorsUpDown.Value = 1;
            rentalTypeComboBox.SelectedIndex = 0;
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid()) return;

            var result = MessageBox.Show("Da li želite da kreirate zajedničku prostoriju?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            CreateCommonRoom();
            ResetForm();
            RefreshCommonRoomsPage();

            MessageBox.Show("Zajednička prostorija je uspešno kreirana.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private bool IsFormValid()
        {
            if (nameTextBox.Text.Trim() == "") return ShowError("Niste uneli naziv prostorije.");
            if (descriptionTextBox.Text.Trim() == "") return ShowError("Niste uneli opis prostorije.");

            return true;
        }
        private bool ShowError(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private void CreateCommonRoom()
        {
            var commonRoom = new CommonRoom(
                buildingId: _building.Id,
                name: nameTextBox.Text.Trim(),
                description: descriptionTextBox.Text.Trim(),
                floorNumber: (int)floorsUpDown.Value!,
                type: GetSelectedRentalType()
            );

            _commonRoomRepository.Create(commonRoom);
        }
        private RentalType GetSelectedRentalType()
        {
            return rentalTypeComboBox.SelectedIndex switch
            {
                0 => RentalType.OneDay,
                1 => RentalType.MultiDay,
                _ => throw new InvalidOperationException("Nepoznat tip iznajmljivanja.")
            };
        }
        private void CommonRoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCommonRoom = (CommonRoom)commonRoomsDataGrid.SelectedItem;

            if (selectedCommonRoom == null) return;

            var approvedDates = _commonRoomRentalRequestRepository.GetApprovedDates(selectedCommonRoom.Id, _building.Id);

            calendar.SelectedDates.Clear();
            foreach (var date in approvedDates)
                calendar.SelectedDates.Add(date);

            calendar.Visibility = Visibility.Visible;
        }
    }
}