using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentMembershipsPage : Page
    {
        private readonly ApartmentMembershipPageViewModel _viewModel;

        public ApartmentMembershipsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new ApartmentMembershipPageViewModel(user, building);
            DataContext = _viewModel;
        }
    }
}