using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public SelectCountryPage(Action<Country> onCountrySelected)
        {
            InitializeComponent();
            DataContext = this;
            _repository = new CountryDbRepository();
            _onCountrySelected = onCountrySelected;
            LoadCountries();
        }

        void LoadCountries()
        {
            Countries.Clear();
            var countries = _repository.GetAll();
            foreach (var country in countries)
                Countries.Add(country);
        }

        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => Pop();

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
