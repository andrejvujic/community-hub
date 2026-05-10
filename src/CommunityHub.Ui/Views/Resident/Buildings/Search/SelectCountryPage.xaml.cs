using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SelectCountryPage : MainNavigatorConsumerPage
    {
        private readonly SelectCountryViewModel _viewModel;


        public SelectCountryPage(Action<Country?> setCountry, Country? selectedCountry = null)
        {
            InitializeComponent();
            _viewModel = new SelectCountryViewModel(
                setCountry: setCountry,
                selectedCountry: selectedCountry
            );
            DataContext = _viewModel;

            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e) => _viewModel.UpdateSearchResults();

        private void CountrySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_viewModel.HasSetSearchText)
                _viewModel.UpdateSearchResults();
        }

        private void CountryItem_Click(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            if (element == null) return;

            var country = element.DataContext as Country;
            if (country == null) return;

            if (_viewModel.IsCountrySelected(country))
            {
                _viewModel.ResetSelectedCountry();
                return;
            }

            _viewModel.UpdateSelectedCountry(country);
            Pop();
        }
    }
}
