
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Views.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly Building _building;
        private readonly User _user;
        private Grid? _selectedSideBarItem = null;
        private CommonRoomRentalRequestsPage _commonRoomRentalRequestsPage;

        public BuildingDetailsPage(User user, Building building)
        {
            InitializeComponent();
            _building = building;
            _user = user;
            _commonRoomRentalRequestsPage = new CommonRoomRentalRequestsPage(_user, _building);
            _commonRoomRentalRequestsPage.CheckAndUpdateRentalDateStatuses();
            _user = user;
            BuildingFrame.Navigate(new BuildingInfoPage(_user, _building));
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
                    BuildingFrame.Navigate(new BuildingInfoPage(_user, _building));
                    break;
                case "ApartmentAccessRequest":
                    BuildingFrame.Navigate(new ApartmentAccessRequestsPage(_user, _building));
                    break;
                case "ApartmentMembership":
                    BuildingFrame.Navigate(new ApartmentMembershipsPage(_user, _building));
                    break;
                case "CommonRoom":
                    BuildingFrame.Navigate(new CommonRoomsPage(_user, _building));
                    break;
                case "CommonRoomRentalRequest":
                    BuildingFrame.Navigate(new CommonRoomRentalRequestsPage(_user, _building));
                    break;
                case "NoticeBoard":
                    BuildingFrame.Navigate(new NoticeBoardPage(_user, _building));
                    break;
                case "SchedulingAssembly":
                    break;
                case "ReportedIssue":
                    BuildingFrame.Navigate(new ReportedIssuesPage(_user, _building));
                    break;
            }
        }
    }
}
