using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using CommunityHub.Ui.Views.Resident.Notifications;
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
            DataContext = _viewModel;
            _viewModel.Initialize();

            OpenBuildingsPage();
        }

        private void OpenBuildingsPage() => LocalFrame.Navigate(
            new BuildingsPage(_viewModel.UserId)
        );

        private void OpenApartmentAccessRequestsPage() => LocalFrame.Navigate(
            new ApartmentAccessRequestsPage(_viewModel.UserId)
        );

        private void TopBar_NotificationsItemClick(object sender, MouseButtonEventArgs e)
                => _viewModel.OpenNotificationsPage();

        private void BottomBar_RequestsItemClick(object sender, MouseButtonEventArgs e) => OpenApartmentAccessRequestsPage();
        private void BottomBar_BuildingsItemClick(object sender, MouseButtonEventArgs e) => OpenBuildingsPage();



    }
}
