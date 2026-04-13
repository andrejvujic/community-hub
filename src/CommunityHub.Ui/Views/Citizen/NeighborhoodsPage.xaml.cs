using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class NeighborhoodsPage : Page
    {
        private readonly long _userId;
        private readonly NeighborhoodRepository _neighborhoodRepository;

        private NeighborhoodSearchParameters? _searchParameters;
        public ObservableCollection<Neighborhood> Neighborhoods { get; } = new ObservableCollection<Neighborhood>();

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;
            _userId = userId;
            _neighborhoodRepository = new NeighborhoodRepository();

            UpdateSearchResults();
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
    }
}
