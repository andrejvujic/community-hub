using System.Windows;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomeWindow : Window
    {
        private readonly HomeWindowViewModel _viewModel;
        public HomeWindow(User user)
        {
            InitializeComponent();
            _viewModel = new HomeWindowViewModel(user);
            DataContext = _viewModel;
        }
    }
}