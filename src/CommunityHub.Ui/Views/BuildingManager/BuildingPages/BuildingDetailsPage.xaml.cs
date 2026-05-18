using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using CommunityHub.Ui.Views.BuildingManager;
using CommunityHub.Ui.Views.BuildingManager.BuildingPages;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly BuildingDetailsPageViewModel _viewModel;
        private Grid? _selectedSideBarItem = null;
        private readonly CommonRoomRentalRequestsPageViewModel _rentalRequestsViewModel;
        private readonly Dictionary<string, Func<Page>> _navigationMap;
        public BuildingDetailsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new BuildingDetailsPageViewModel(user, building);
            DataContext = _viewModel;
            _rentalRequestsViewModel = new CommonRoomRentalRequestsPageViewModel(_viewModel.User, _viewModel.Building);
            _rentalRequestsViewModel.AutoProcessRentalRequestStatuses();
            BuildingFrame.Navigate(new BuildingInfoPage(_viewModel.User, _viewModel.Building));


            _navigationMap = new Dictionary<string, Func<Page>>
            {
                ["Info"] = () => new BuildingInfoPage(_viewModel.User, _viewModel.Building),
                ["ApartmentAccessRequest"] = () => new ApartmentAccessRequestsPage(_viewModel.User, _viewModel.Building),
                ["ApartmentMembership"] = () => new ApartmentMembershipsPage(_viewModel.User, _viewModel.Building),
                ["CommonRoom"] = () => new CommonRoomsPage(_viewModel.User, _viewModel.Building),
                ["CommonRoomRentalRequest"] = () => new CommonRoomRentalRequestsPage(_viewModel.User, _viewModel.Building),
                ["NoticeBoard"] = () => new NoticeBoardPage(_viewModel.User, _viewModel.Building),
                ["ReportedIssue"] = () => new ReportedIssuesPage(_viewModel.User, _viewModel.Building)
            };
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
            if (tag != null && _navigationMap.TryGetValue(tag, out var createPage))
            {
                BuildingFrame.Navigate(createPage());
            }
        }
    }
}