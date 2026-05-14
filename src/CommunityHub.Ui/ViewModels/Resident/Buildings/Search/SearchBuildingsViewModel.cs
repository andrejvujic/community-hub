using CommunityHub.Application;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Buildings.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Buildings;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SearchBuildingsViewModel : INotifyPropertyChanged
    {
        public long UserId { get; }

        private readonly BuildingService _buildingService = new BuildingService(Injector.CreateInstance<IBuildingRepository>());

        public ObservableCollection<Building> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SearchBuildingsViewModel(long userId)
        {
            UserId = userId;
        }


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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
