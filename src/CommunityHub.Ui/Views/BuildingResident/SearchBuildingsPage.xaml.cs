using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SearchBuildingsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly BuildingRepository _repository;
        private BuildingSearchParameters? _currentSearchParameters;
        public ObservableCollection<Building> Buildings { get; } = new ObservableCollection<Building>();

        public SearchBuildingsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _repository = new BuildingRepository();
            UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e)
            => Push(
                new SearchCriteriaPage(
                        OnSearchParametersChanged,
                        initialSearchParameters: _currentSearchParameters
                    )
               );
        
        private void OnSearchParametersChanged(BuildingSearchParameters searchParameters)
        {
            _currentSearchParameters = searchParameters;
            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            MainContent.Visibility = Visibility.Hidden;
            EmptyContent.Visibility = Visibility.Hidden;

            Buildings.Clear();
            var searchResultBuildings = _repository.Search(_currentSearchParameters ?? BuildingSearchParameters.Default());

            foreach (var building in searchResultBuildings)
                Buildings.Add(building);

            if (Buildings.Count > 0) {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }
    }
}
