using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Matching;
using CommunityHub.Application.Helpers.Search;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class NeighborhoodsPage : Page
    {
        private readonly long _userId;
        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();
        private readonly NeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository = new NeighborhoodAccessRequestRepository();
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly NeighborhoodMembershipRepository _neighborhoodMembershipRepository = new NeighborhoodMembershipRepository();

        private NeighborhoodSearchParameters? _searchParameters;

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            this.DataContext = _citizenRepository.GetById(_userId);

            UpdateSearchResults();
        }

        private void NeighborhoodsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = NeighborhoodsGrid.SelectedItem as Neighborhood;
            if (selected != null)
            {
                LoadStreetSections(selected);
            }
        }

        private void LoadStreetSections(Neighborhood neighborhood)
        {
            if (neighborhood.StreetSections.Any() || neighborhood.Id == 0) return;

            var loaded = _streetSectionRepository.GetByNeighborhoodId(neighborhood.Id);
            neighborhood.StreetSections.Clear();
            foreach (var s in loaded) neighborhood.StreetSections.Add(s);
        }

        private void UpdateSearchResults()
        {
            var neighborhoods = _neighborhoodRepository.Search(_searchParameters ?? NeighborhoodSearchParameters.Empty());
            NeighborhoodsGrid.ItemsSource = neighborhoods;
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new NeighborhoodSearchParametersWindow(_searchParameters);
            if (window.ShowDialog() == true)
            {
                _searchParameters = window.Result;
                UpdateSearchResults();
            }
        }

        private void RequestAccessButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = NeighborhoodsGrid.SelectedItem as Neighborhood;
            if (selected == null)
            {
                MessageBox.Show("Izaberite kvart pre slanja zahteva.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_neighborhoodAccessRequestRepository.HasPendingRequest(_userId, selected.Id))
            {
                MessageBox.Show("Već postoji zahtev za pristup ovom kvartu.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new RequestNeighborhoodAccessWindow(selected.Name ?? string.Empty);
            dlg.Owner = Window.GetWindow(this);
            if (!(dlg.ShowDialog() == true && dlg.Confirmed)) return;

            var citizen = _citizenRepository.GetById(_userId)!;

            if (NeighborhoodAddressMatcher.AddressBelongsToNeighborhood(citizen.Address, selected.StreetSections))
            {
                CreateNeighborhoodMembership(citizen, selected);
            }
            else
            {
                CreateNeighborhoodAccessRequest(citizen, selected);
            }
        }



        private void CreateNeighborhoodMembership(CommunityHub.Application.Domain.Citizen citizen, Neighborhood neighborhood)
        {
            MessageBox.Show("Vaša adresa pripada ovom kvartu. Bićete automatski pridruženi kvartu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);

            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                neighborhood
            );

            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            _citizenRepository.UpdateMembership(citizen.Id, createdMembership.Id);

            AutoRejectPendingRequests(citizen, neighborhood.Id);

            MessageBox.Show("Uspešno ste pridruženi kvartu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DataContext = _citizenRepository.GetById(_userId);
        }

        private void AutoRejectPendingRequests(CommunityHub.Application.Domain.Citizen citizen, long approvedNeighborhoodId)
        {
            var pending = _neighborhoodAccessRequestRepository.GetWaitingByCitizenId(citizen.Id);
            foreach (var r in pending.Where(r => r.Neighborhood.Id != approvedNeighborhoodId))
            {
                var updated = r.CopyWith(status: Status.Rejected, comment: "Automatski odbijen: građanin je pridružen drugom kvartu.");
                _neighborhoodAccessRequestRepository.Update(updated);
            }
        }

        private void CreateNeighborhoodAccessRequest(CommunityHub.Application.Domain.Citizen citizen, Neighborhood neighborhood)
        {
            MessageBox.Show("Vaša adresa ne pripada ovom kvartu. Biće kreiran zahtev za pristup.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            
            var request = new NeighborhoodAccessRequest(
                citizen,
                neighborhood,
                Status.Waiting,
                DateTime.Now,
                null
            );

            bool created = _neighborhoodAccessRequestRepository.Create(request);
            if (created)
            {
                MessageBox.Show("Zahtev za pristup kvartu je uspešno kreiran.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri kreiranju zahteva.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyAccessRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MyAccessRequestsWindow(_userId);
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}
