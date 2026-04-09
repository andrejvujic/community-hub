using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SelectCityPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly CityDbRepository _repository;
        private readonly Action<City?> _onCitySelected;
        private readonly Country _selectedCountry;
        public City? SelectedCity { get; private set; }
        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();

        public string SearchText { get; set; } = string.Empty;
        private CitySearchParameters GetSearchParameters() => new CitySearchParameters(
            country: _selectedCountry,
            name: SearchText
        );

        public SelectCityPage(
            Action<City?> onCitySelected,
            Country selectedCountry,
            City? selectedCity = null
        )
        {
            InitializeComponent();
            DataContext = this;

            _repository = new CityDbRepository();

            _onCitySelected = onCitySelected;
            _selectedCountry = selectedCountry;
            SelectedCity = selectedCity;

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
            var element = sender as FrameworkElement;
            if (element == null) return;

            var city = element.DataContext as City;
            if (city == null) return;

            if (city == SelectedCity)
            {
                // If the selected city is clicked again, it should be deselcted.
                SelectedCity = null;
                OnPropertyChanged(nameof(SelectedCity));

                _onCitySelected.Invoke(null);
                return;
            }

            _onCitySelected.Invoke(city);
            Pop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}