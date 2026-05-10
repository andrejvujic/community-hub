using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SelectCityPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly SelectCityViewModel _viewModel;

        public SelectCityPage(
            Action<City?> setCity,
            Country selectedCountry,
            City? selectedCity = null
        )
        {
            InitializeComponent();
            _viewModel = new SelectCityViewModel(
                setCity: setCity,
                selectedCountry: selectedCountry,
                selectedCity: selectedCity
            );
            DataContext = _viewModel;

            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e) => _viewModel.UpdateSearchResults();

        private void CitySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_viewModel.HasSetSearchText)
                _viewModel.UpdateSearchResults();
        }

        private void CityItem_Click(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var city = element.DataContext as City;
            if (city == null) return;

            if (_viewModel.IsCitySelected(city))
            {
                _viewModel.ResetSelectedCity();
                return;
            }

            _viewModel.UpdateSelectedCity(city);
            Pop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}