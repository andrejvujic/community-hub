using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Views.BuildingManager.BuildingPages;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly BuildingDetailsPageViewModel _viewModel;
        private Grid? _selectedSideBarItem = null;
        private readonly CommonRoomRentalRequestsPageViewModel _rentalRequestsViewModel;

        public BuildingDetailsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new BuildingDetailsPageViewModel(user, building);
            DataContext = _viewModel;
            _rentalRequestsViewModel = new CommonRoomRentalRequestsPageViewModel(_viewModel.User, _viewModel.Building);
            _rentalRequestsViewModel.CheckAndUpdateRentalDateStatuses();
            BuildingFrame.Navigate(new BuildingInfoPage(_viewModel.User, _viewModel.Building));
        }
        private void SideBarItem_Click(object sender, MouseButtonEventArgs e)
        {
            var clicked = sender as Grid;
            if (clicked == null)
                return;

            if (!string.IsNullOrEmpty(clicked.Name))
                SetSideBarItemBackground(clicked);
            
            string? tag = clicked.Tag?.ToString();
            NavigateTo(tag);
        }
        private void SetSideBarItemBackground(Grid item)
        {
            if (_selectedSideBarItem != null)
                _selectedSideBarItem.ClearValue(BackgroundProperty);

            _selectedSideBarItem = item;
            _selectedSideBarItem.Background = Brushes.LightGray;
        }
        private void NavigateTo(string? tag)
        {
            switch (tag)
            {
                case "Info":
                    BuildingFrame.Navigate(new BuildingInfoPage(_viewModel.User, _viewModel.Building));
                    break;
                case "ApartmentAccessRequest":
                    BuildingFrame.Navigate(new ApartmentAccessRequestsPage(_viewModel.User, _viewModel.Building));
                    break;
                case "ApartmentMembership":
                    BuildingFrame.Navigate(new ApartmentMembershipsPage(_viewModel.User, _viewModel.Building));
                    break;
                case "CommonRoom":
                    BuildingFrame.Navigate(new CommonRoomsPage(_viewModel.User, _viewModel.Building));
                    break;
                case "CommonRoomRentalRequest":
                    BuildingFrame.Navigate(new CommonRoomRentalRequestsPage(_viewModel.User, _viewModel.Building));
                    break;
                case "NoticeBoard":
                    BuildingFrame.Navigate(new NoticeBoardPage(_viewModel.User, _viewModel.Building));
                    break;
                case "SchedulingAssembly":
                    break;
                case "ReportedIssue":
                    BuildingFrame.Navigate(new ReportedIssuesPage(_viewModel.User, _viewModel.Building));
                    break;
            }
        }
    }
}