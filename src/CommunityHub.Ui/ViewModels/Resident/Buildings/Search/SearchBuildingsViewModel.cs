using CommunityHub.Application;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Buildings;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SearchBuildingsViewModel : BaseViewModel
    {
        private readonly long _userId;
        private readonly BuildingService _buildingService = new BuildingService();
        private readonly Action _scrollSearchResultsToTop;

        public ObservableCollection<BuildingDto> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SearchBuildingsViewModel(
            long userId,
            MainNavigationConsumerPage page,
            Action scrollSearchResultsToTop
        ) : base(page)
        {
            _userId = userId;
            _scrollSearchResultsToTop = scrollSearchResultsToTop;

            OpenSearchCriteriaCommand = new(OpenSearchCriteriaPage);
            ResetSearchCommand = new(ResetSearch);
            OpenBuildingDetailsCommand = new(OpenBuildingDetailsPage);
            OpenRequestApartmentAccessCommand = new(OpenRequestApartmentAccessPage);
        }

        public RelayCommand OpenSearchCriteriaCommand { get; }
        public RelayCommand ResetSearchCommand { get; }
        public RelayCommand<BuildingDto> OpenBuildingDetailsCommand { get; }
        public RelayCommand<BuildingDto> OpenRequestApartmentAccessCommand { get; }

        private void OpenSearchCriteriaPage()
            => Push(
                new SearchCriteriaPage(SetSearchParameters, SearchParameters),
                onPop: _ => UpdateSearchResults()
            );

        private void OpenBuildingDetailsPage(BuildingDto? building)
        {
            if (building is null) return;
            Push(new BuildingDetailsPage(building));
        }

        private void OpenRequestApartmentAccessPage(BuildingDto? building)
        {
            if (building is null) return;
            Push(new RequestApartmentAccessPage(_userId, building));
        }


        private BuildingSearchParameters? _searchParameters;
        private BuildingSearchParameters? SearchParameters
        {
            get => _searchParameters;
            set
            {
                if (_searchParameters == value) return;
                _searchParameters = value;
                OnPropertyChanged(nameof(SearchParameters));
                OnPropertyChanged(nameof(HasSetSearchParameters));
            }
        }

        public bool HasSetSearchParameters => SearchParameters is not null;

        private void SetSearchParameters(BuildingSearchParameters searchParameters)
            => SearchParameters = searchParameters;

        private void ResetSearchParameters()
            => SearchParameters = null;

        private void ResetSearch()
        {
            ResetSearchParameters();
            UpdateSearchResults();
            _scrollSearchResultsToTop();
        }

        public void UpdateSearchResults()
        {
            var buildings = _buildingService.Search(
                _searchParameters ?? BuildingSearchParameters.Empty()
            );

            SearchResults.Clear();
            foreach (var building in buildings)
                SearchResults.Add(building);

            OnPropertyChanged(nameof(HasSearchResults));
        }

    }
}
