using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.ViewModels.BuildingManager;
using System.Windows;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomeWindow : Window
    {
        private readonly HomeWindowViewModel _viewModel;
        public HomeWindow(User user)
        {
            InitializeComponent();

            IAppNavigationService navigationService = new AppNavigationService();

            _viewModel = new HomeWindowViewModel(user, navigationService);
            DataContext = _viewModel;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}