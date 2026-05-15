using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain;
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
 
            NeighborhoodAccessRequestStatus? selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Tag as Status?;
            _viewModel.LoadRequests(selectedStatus);
        }
 
        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedRequest = RequestsDataGrid.SelectedItem as NeighborhoodAccessRequestDto;
        }
 
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null) return;
 
            var req = _viewModel.SelectedRequest;
            var dialog = new RejectRequestWindow(req.Citizen.Name, req.Citizen.Surname, req.Neighborhood.Name);
            dialog.Owner = Window.GetWindow(this);
 
            if (dialog.ShowDialog() != true) return;
 
            _viewModel.Reject(dialog.Comment);
 
            MessageBox.Show(
                $"Korisniku {req.Citizen.Name} {req.Citizen.Surname} je odbijen zahtev za pristup kvartu {req.Neighborhood.Name}.",
                "Odbijen zahtev");
        }
 
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null) return;
 
            var req = _viewModel.SelectedRequest;
 
            var result = MessageBox.Show(
                $"Da li ste sigurni da želite da prihvatite zahtev korisnika {req.Citizen.Name} {req.Citizen.Surname} za pristup kvartu {req.Neighborhood.Name}?",
                "Potvrda prihvatanja",
                MessageBoxButton.YesNo);
 
            if (result != MessageBoxResult.Yes) return;
 
            _viewModel.Accept();
 
            MessageBox.Show(
                $"Korisnik {req.Citizen.Name} {req.Citizen.Surname} je postao novi član kvarta {req.Neighborhood.Name}.",
                "Prihvaćen zahtev");
        }
    }
}
