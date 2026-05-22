using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class MyBuildingsPage : Page
    {
        private readonly MyBuildingsPageViewModel _viewModel;
        public MyBuildingsPage(User user)
        {
            InitializeComponent();
            _viewModel = new MyBuildingsPageViewModel(user);

            _viewModel.Navigate = page =>
            {
                var window = Window.GetWindow(this) as HomeWindow;
                if (window != null)
                    window.HomeFrame.Navigate(page);
            };

            DataContext = _viewModel;
        }
    }
}