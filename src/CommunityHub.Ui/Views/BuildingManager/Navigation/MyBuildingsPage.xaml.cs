using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class MyBuildingsPage : Page
    {
        private readonly MyBuildingsPageViewModel _viewModel;
        public MyBuildingsPage(User user, IAppNavigationService navigationService)
        {
            InitializeComponent();
            _viewModel = new MyBuildingsPageViewModel(user, navigationService);

            DataContext = _viewModel;
        }
    }
}