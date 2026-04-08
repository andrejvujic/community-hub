using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SelectCityPage : MainNavigatorConsumerPage
    {
        private readonly CityDbRepository _repository;
        private readonly Country _selectedCountry;
        private readonly Action<City> _onCitySelected;

        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();

        public string SearchText { get; set; } = string.Empty;
        private CitySearchParameters GetSearchParameters() => new CitySearchParameters(SearchText);

        public SelectCityPage(Country selectedCountry, Action<City> onCitySelected)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new CityDbRepository();
            _selectedCountry = selectedCountry;
            _onCitySelected = onCitySelected;

            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            MainContent.Visibility = Visibility.Hidden;
            EmptyContent.Visibility = Visibility.Hidden;

            LoadSearchResults();

            if (Cities.Count > 0)
            {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }

        private void LoadSearchResults()
        {
            var cities = _repository.Search(GetSearchParameters());
            Cities.Clear();
            foreach (var city in cities)
                Cities.Add(city);
        }

        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => UpdateSearchResults();

        private void CitySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                UpdateSearchResults();
        }

        private void CityItem_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null) return;

            var selectedCity = grid.DataContext as City;
            if (selectedCity == null) return;

            _onCitySelected.Invoke(selectedCity);
            Pop();
        }
    }
}