using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomRentalRequestsPage : Page
    {
        public CommonRoomRentalRequestsPageViewModel _viewModel { get; }
        private Border? _selectedCard;

        public CommonRoomRentalRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new CommonRoomRentalRequestsPageViewModel(user, building);
            DataContext = _viewModel;
        }
        private void CommonRoomRentalRequestsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshRentalRequests();
        }
        private void RentalRequestCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedCard != null)
                _selectedCard.Background = Brushes.White;

            _selectedCard = sender as Border;
            if (_selectedCard != null)
                _selectedCard.Background = Brushes.LightGray;

            _viewModel.SelectedRentalRequest = _selectedCard?.DataContext as CommonRoomRentalRequestDto;
        }
        private void LeftArrow_Click(object sender, MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
        private void OneDaySuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not DateTime day)
                return;

            _viewModel.ApproveOneDaySuggestion(day);
        }
        private void MultiDaySuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not SuggestedDateRangeViewModel range)
                return;

            _viewModel.AcceptMultiDaySuggestion(range.StartDate, range.EndDate);
        }
    }
}