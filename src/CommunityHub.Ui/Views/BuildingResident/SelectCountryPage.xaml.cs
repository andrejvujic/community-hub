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
    public partial class SelectCountryPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly CountryDbRepository _repository;
        private readonly Action<Country?> _onCountrySelected;
        public Country? SelectedCountry { get; private set; }
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        public string SearchText { get; set; } = string.Empty;
        private CountrySearchParameters GetSearchParameters() => new CountrySearchParameters(name: SearchText);

        public SelectCountryPage(Action<Country?> onCountrySelected, Country? selectedCountry = null)
        {
            InitializeComponent();
            DataContext = this;

            _repository = new CountryDbRepository();
            _onCountrySelected = onCountrySelected;
            SelectedCountry = selectedCountry;

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
            var countries = _repository.Search(GetSearchParameters());
            Countries.Clear();
            foreach (var country in countries)
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

            var country = grid.DataContext as Country;
            if (country == null) return;

            if (country == SelectedCountry)
            {
                // If the selected city is clicked again, it should be deselcted.
                SelectedCountry = null;
                OnPropertyChanged(nameof(SelectedCountry));
                _onCountrySelected.Invoke(null);
                return;
            }

            _onCountrySelected.Invoke(country);
            Pop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
