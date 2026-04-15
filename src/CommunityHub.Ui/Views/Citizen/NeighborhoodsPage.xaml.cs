using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class NeighborhoodsPage : Page
    {
        private readonly long _userId;
        private readonly NeighborhoodRepository _neighborhoodRepository;
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();

        private NeighborhoodSearchParameters? _searchParameters;
        public ObservableCollection<Neighborhood> Neighborhoods { get; } = new ObservableCollection<Neighborhood>();
        private Neighborhood? _selectedNeighborhood;

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            _neighborhoodRepository = new NeighborhoodRepository();

            UpdateSearchResults();

            NeighborhoodsGrid.SelectionChanged += NeighborhoodsGrid_SelectionChanged;
        }

        private void NeighborhoodsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsGrid.SelectedItem as Neighborhood;
            UpdateRightPanel();
        }

        private void UpdateRightPanel()
        {
            if (_selectedNeighborhood == null)
            {
                ShowPlaceholder();
                return;
            }

            ShowDetails(_selectedNeighborhood);
        }

        private void LoadStreetSections(Neighborhood neighborhood)
        {
            if (neighborhood == null || neighborhood.StreetSections.Any() || neighborhood.Id == 0) return;

            var loaded = _streetSectionRepository.GetByNeighborhoodId(neighborhood.Id);
            neighborhood.StreetSections.Clear();
            foreach (var s in loaded) neighborhood.StreetSections.Add(s);
        }

        private void ShowPlaceholder()
        {
            PlaceholderPanel.Visibility = Visibility.Visible;
            DetailsPanel.Visibility = Visibility.Collapsed;
            StreetsList.ItemsSource = null;
            ImagesList.ItemsSource = null;
            DetailsName.Text = string.Empty;
            DetailsLocation.Text = string.Empty;
            DetailsDescription.Text = string.Empty;
        }

        private void ShowDetails(Neighborhood neighborhood)
        {
            PlaceholderPanel.Visibility = Visibility.Collapsed;
            DetailsPanel.Visibility = Visibility.Visible;

            DetailsName.Text = neighborhood.Name ?? string.Empty;
            DetailsLocation.Text = FormatLocation(neighborhood);
            DetailsDescription.Text = neighborhood.Description ?? string.Empty;

            LoadStreetSections(neighborhood);
            PopulateStreets(neighborhood);
            PopulateImages(neighborhood);
        }

        private string FormatLocation(Neighborhood neighborhood)
        {
            var locationParts = new[] { neighborhood.City?.Name, neighborhood.Country?.Name }
                .Where(part => !string.IsNullOrWhiteSpace(part));

            return string.Join(", ", locationParts);
        }

        private void PopulateStreets(Neighborhood neighborhood)
        {
            var streetStrings = neighborhood.StreetSections
                .OrderBy(s => s.Street.Name)
                .ThenBy(s => s.StartNumber)
                .Select(s => $"{s.Street.Name} {s.StartNumber}-{s.EndNumber}")
                .ToList();

            StreetsList.ItemsSource = streetStrings;
        }

        private void PopulateImages(Neighborhood neighborhood)
        {
            ImagesList.ItemsSource = neighborhood.Images;
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
            if (_selectedNeighborhood == null)
            {
                MessageBox.Show("Izaberite kvart pre slanja zahteva.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new RequestNeighborhoodAccessWindow(_selectedNeighborhood.Name ?? "");
            dlg.Owner = Window.GetWindow(this);
            dlg.ShowDialog();
        }
    }
}
