using System.Collections.Generic;
using System.Windows.Controls;
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

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            _neighborhoodRepository = new NeighborhoodRepository();

            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            var neighborhoods = _neighborhoodRepository.Search(_searchParameters ?? NeighborhoodSearchParameters.Empty());
            NeighborhoodsGrid.ItemsSource = neighborhoods;
        }
    }
}
