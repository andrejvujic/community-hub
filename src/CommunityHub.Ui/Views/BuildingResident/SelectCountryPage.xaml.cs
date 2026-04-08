using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace CommunityHub.Ui.Views.BuildingResident
{
    /// <summary>
    /// Interaction logic for PickCountry.xaml
    /// </summary>
    public partial class SelectCountryPage : MainNavigatorConsumerPage
    {
        private readonly CountryDbRepository _repository;
        private readonly Action<Country> _onCountrySelected;

        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        public string SearchText { get; set; } = string.Empty;
        private CountrySearchParameters GetSearchParameters() => new CountrySearchParameters(SearchText);

        public SelectCountryPage(Action<Country> onCountrySelected)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new CountryDbRepository();
            _onCountrySelected = onCountrySelected;

            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            MainContent.Visibility = Visibility.Hidden;
            EmptyContent.Visibility = Visibility.Hidden;

            LoadSearchResults();

            if (Countries.Count > 0)
            {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }

        private void LoadSearchResults()
        {
            var searchResultCountries = _repository.Search(GetSearchParameters());
            Countries.Clear();
            foreach (var country in searchResultCountries)
                Countries.Add(country);
        }

        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => UpdateSearchResults();

        private void CountrySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                UpdateSearchResults();
        }

        private void CountryItem_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null) return;

            var selectedCountry = grid.DataContext as Country;
            if (selectedCountry == null) return;

            _onCountrySelected.Invoke(selectedCountry);
            Pop();
        }
    }
}
