using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident;
using CommunityHub.Ui.Views;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident
{
    public partial class HomePage : MainNavigationConsumerPage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage(long userId)
        {
            InitializeComponent();
            
            _viewModel = new HomeViewModel(userId, this);
            _viewModel.LogoutRequested += OnLogoutRequested;
            DataContext = _viewModel;

            LocalFrame.Navigate(new BuildingsPage(_viewModel.UserId));
        }

        private void OpenBuildingsPageButton_Click(object sender, MouseButtonEventArgs e)
            => LocalFrame.Navigate(new BuildingsPage(_viewModel.UserId));

        private void OpenApartmentAccessRequestsPageButton_Click(object sender, MouseButtonEventArgs e)
            => LocalFrame.Navigate(new ApartmentAccessRequestsPage(_viewModel.UserId));

        private void OnLogoutRequested(object? sender, EventArgs e)
        {
            new LogInForm().Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
