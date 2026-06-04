using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomRentalRequestsPage : Page
    {
        public CommonRoomRentalRequestsPageViewModel _viewModel { get; }
        public CommonRoomRentalRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new CommonRoomRentalRequestsPageViewModel(user, building);
            DataContext = _viewModel;
        }
    }
}