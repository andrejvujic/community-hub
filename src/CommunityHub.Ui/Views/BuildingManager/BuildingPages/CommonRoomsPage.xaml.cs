using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Application.Domain.Users;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomsPage : Page
    {
        private readonly CommonRoomsPageViewModel _viewModel;
        private bool _isSyncingCalendar = false;
        public CommonRoomsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new CommonRoomsPageViewModel(user, building);
            DataContext = _viewModel;
        }
        private void CommonRoomsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCommonRooms();
            SyncCalendar();
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li želite da otkažete kreiranje zajedničke prostorije?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            _viewModel.ResetForm();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidateForm()) return;

            var result = MessageBox.Show("Da li želite da kreirate zajedničku prostoriju?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            _viewModel.CreateCommonRoom();
            _viewModel.ResetForm();
            _viewModel.LoadCommonRooms();

            MessageBox.Show("Zajednička prostorija je uspešno kreirana.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void CommonRoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCommonRoom = commonRoomsDataGrid.SelectedItem as CommonRoomDto;

            _viewModel.HandleSelectedCommonRoomChanged(selectedCommonRoom);
            SyncCalendar();
        }
        private void SyncCalendar()
        {
            _isSyncingCalendar = true;

            calendar.SelectedDates.Clear();
            foreach (var date in _viewModel.ReservedDates)
                calendar.SelectedDates.Add(date);

            calendar.Visibility = _viewModel.IsCalendarVisible ? Visibility.Visible : Visibility.Collapsed;

            _isSyncingCalendar = false;
        }
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_isSyncingCalendar) return;

            SyncCalendar();
        }
    }
}