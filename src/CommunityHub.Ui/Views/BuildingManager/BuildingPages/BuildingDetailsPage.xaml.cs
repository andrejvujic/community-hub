using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly BuildingDetailsPageViewModel _viewModel;
        
        public BuildingDetailsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new BuildingDetailsPageViewModel(user, building);
            DataContext = _viewModel;           
        } 
    }
}