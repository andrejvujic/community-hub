using System.Windows.Controls;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomePage : Page
    {
        private readonly HomePageViewModel _viewModel;
        public HomePage(User user)
        {
            InitializeComponent();
            _viewModel = new HomePageViewModel(user);
            DataContext = _viewModel;
        }
    }
}