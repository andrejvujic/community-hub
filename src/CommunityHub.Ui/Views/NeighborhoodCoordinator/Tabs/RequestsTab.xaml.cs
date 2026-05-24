using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
 
namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class RequestsTab : UserControl
    {
        private RequestsTabViewModel _viewModel = null!;
 
        public RequestsTab()
        {
            InitializeComponent();
        }
 
        public void Initialize(long coordinatorId)
        {
            _viewModel = new RequestsTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadRequests(null);
        }
 
        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null) return;
 
            NeighborhoodAccessRequestStatus? selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Tag as NeighborhoodAccessRequestStatus?;
            _viewModel.LoadRequests(selectedStatus);
        }
 
        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedRequest = RequestsDataGrid.SelectedItem as NeighborhoodAccessRequestDto;
        }
 
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null) return;
 
            var request = _viewModel.SelectedRequest;
            var dialog = new RejectRequestWindow(request.Citizen.Name, request.Citizen.Surname, request.Neighborhood.Name);
            dialog.Owner = Window.GetWindow(this);
 
            if (dialog.ShowDialog() != true) return;
 
            _viewModel.Reject(dialog.Comment);

            new InfoDialog("Odbijen zahtev", $"Korisniku {request.Citizen.Name} {request.Citizen.Surname} je odbijen zahtev za pristup kvartu {request.Neighborhood.Name}.", Window.GetWindow(this)).ShowDialog();
        }
 
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null) return;
 
            var request = _viewModel.SelectedRequest;

            var confirmDialog = new ConfirmDialog(
                "Potvrda prihvatanja",
                $"Da li ste sigurni da želite da prihvatite zahtev korisnika {request.Citizen.Name} {request.Citizen.Surname} za pristup kvartu {request.Neighborhood.Name}?",
                owner: Window.GetWindow(this));

            confirmDialog.ShowDialog();

            if (!confirmDialog.Confirmed) return;

            _viewModel.Accept();

            new InfoDialog("Prihvaćen zahtev", $"Korisnik {request.Citizen.Name} {request.Citizen.Surname} je postao novi član kvarta {request.Neighborhood.Name}.", Window.GetWindow(this)).ShowDialog();
        }
    }
}
