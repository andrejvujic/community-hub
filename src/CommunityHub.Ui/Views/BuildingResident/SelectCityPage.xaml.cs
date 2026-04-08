using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SelectCityPage : MainNavigatorConsumerPage
    {
        private readonly CityDbRepository _repository;
        private readonly Country _selectedCountry;
        private readonly Action<City> _onCitySelected;

        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();

        public SelectCityPage(Country selectedCountry, Action<City> onCitySelected)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new CityDbRepository();
            _selectedCountry = selectedCountry;
            _onCitySelected = onCitySelected;

            LoadCities();
        }

        private void LoadCities()
        {
            Cities.Clear();
            var cities = _repository.GetByCountryId(_selectedCountry.Id);
            foreach (var city in cities)
                Cities.Add(city);
        }

        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => Pop();

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
