using CommunityHub.Ui.ViewModels.Citizen;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class HomeWindow : Window
    {
        public HomeWindow(long userId)
        {
            InitializeComponent();
            MainFrame.Navigate(new MainMenuPage(userId));

            Loaded += (_, _) =>
            {
                var viewModel = HomeViewModel.CreateWithResources(userId, this);
                viewModel.TabNavigationRequested += (_, e) => MainFrame.Navigate(e.Page);
                viewModel.CloseRequested += (_, _) => Close();
                DataContext = viewModel;
            };
        }
    }
}
