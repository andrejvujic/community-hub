using CommunityHub.Application;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Buildings;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SearchBuildingsViewModel : BaseViewModel
    {
        public long UserId { get; }

        private readonly BuildingService _buildingService = new BuildingService();

        public ObservableCollection<BuildingDto> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SearchBuildingsViewModel(long userId, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
        }

        public void OpenSearchCriteriaPage()
            => Push(
                new SearchCriteriaPage(SetSearchParameters, SearchParameters),
                onPop: _ => UpdateSearchResults()
            );


        private BuildingSearchParameters? _searchParameters;
        public BuildingSearchParameters? SearchParameters
        {
            get => _searchParameters;
            private set
            {
                if (_searchParameters == value) return;
                _searchParameters = value;
                OnPropertyChanged(nameof(SearchParameters));
                OnPropertyChanged(nameof(HasSetSearchParameters));
            }
        }

        public bool HasSetSearchParameters => SearchParameters is not null;

        public void SetSearchParameters(BuildingSearchParameters searchParameters)
            => SearchParameters = searchParameters;

        public void ResetSearchParameters()
            => SearchParameters = null;

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
