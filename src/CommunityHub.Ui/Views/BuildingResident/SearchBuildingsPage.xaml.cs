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
        private BuildingSearchParameters? _searchParameters;
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
                        initialSearchParameters: _searchParameters
                    )
               );
        
        private void OnSearchParametersChanged(BuildingSearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
            UpdateSearchResults();
        }

        private void LoadSearchResults()
        {
            var buildings = _repository.Search(_searchParameters ?? BuildingSearchParameters.Empty());
            Buildings.Clear();
            foreach (var building in buildings)
                Buildings.Add(building);
        }   

        private void UpdateSearchResults()
        {
            BuildingsScrollViewer.ScrollToTop();
            ResetSearchButton.Visibility = _searchParameters == null ? Visibility.Hidden : Visibility.Visible;

            MainContent.Visibility = Visibility.Hidden;
            EmptyContent.Visibility = Visibility.Hidden;

            LoadSearchResults();

            if (Buildings.Count > 0) {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            _searchParameters = null;
            UpdateSearchResults();
        }
    }
}
