using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Buildings;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{

    public partial class SchedulingAssemblyPage : Page
    {
       private readonly SchedulingAssemblyPageViewModel _viewModel;
        public SchedulingAssemblyPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new SchedulingAssemblyPageViewModel(user, building);
            DataContext = _viewModel;
        }
    }
}