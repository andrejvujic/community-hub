using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomeWindow : Window
    {
        private readonly HomeWindowViewModel _viewModel;
        private Grid? _selectedNavBarItem = null;
        public HomeWindow(User user)
        {
            InitializeComponent();
            _viewModel = new HomeWindowViewModel(user);
            DataContext = _viewModel;
            HomeFrame.Navigate(new HomePage(user));
        }
        private void NavBarItem_Click(object sender, MouseButtonEventArgs e)
        {
            var clicked = sender as Grid;
            if (clicked == null)
                return;

            if (!string.IsNullOrEmpty(clicked.Name) && clicked.Name.StartsWith("navBar"))
                SetNavBarItemBackground(clicked);
            
            string? tag = clicked.Tag?.ToString();
            NavigateTo(tag);
        }
        private void SetNavBarItemBackground(Grid item)
        {
            if (_selectedNavBarItem != null)
                _selectedNavBarItem.ClearValue(BackgroundProperty);

            _selectedNavBarItem = item;
            _selectedNavBarItem.Background = Brushes.LightGray;
        }
        private void NavigateTo(string? tag)
        {
            switch (tag)
            {
                case "Home":
                    HomeFrame.Navigate(new HomePage(_viewModel.User));
                    break;
                case "Assembly":
                    HomeFrame.Navigate(new AssemblyPage(_viewModel.User));
                    break;
                case "MyBuildings":
                    HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
                    break;
                case "Registration":
                    HomeFrame.Navigate(new RegistrationPage(_viewModel.User));
                    break;
                case "Help":
                    //TODO: Implementirati Help Page
                    break;
                case "LogOut":
                    HandleLogOut();
                    break;
            }
        }
        private void HandleLogOut()
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da se odjavite?", "Pažnja", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LogInForm logInForm = new LogInForm();
                logInForm.Show();
                this.Close();
            }
        }
    }
}