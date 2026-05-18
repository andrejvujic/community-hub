using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentAccessRequestsPage : Page
    {
        private readonly ApartmentAccessRequestPageViewModel _viewModel;
        public ApartmentAccessRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new ApartmentAccessRequestPageViewModel(user, building);
            DataContext = _viewModel;
            _viewModel.LoadAccessRequests(null);
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni da želite otkazati promenu statusa zahteva?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
               _viewModel.ResetSelectionAndForm();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Status? newStatus = _viewModel.GetSelectedStatus();

            if (newStatus == null)
            {
                MessageBox.Show("Molimo odaberite status zahteva.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                "Da li ste sigurni da želite promeniti status zahteva?",
                "Potvrda",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            bool success = _viewModel.ConfirmStatusChange();

            if(success)
                MessageBox.Show("Status zahteva je uspešno promenjen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}