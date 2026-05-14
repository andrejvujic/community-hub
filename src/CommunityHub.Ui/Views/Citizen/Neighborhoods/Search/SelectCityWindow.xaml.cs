using System.Windows;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SelectCityWindow : Window
    {
        private readonly SelectCityViewModel _viewModel;
        public City? SelectedCity { get; private set; }

        public SelectCityWindow(Country? selectedCountry = null)
        {
            InitializeComponent();
            _viewModel = new SelectCityViewModel(selectedCountry);
            DataContext = _viewModel;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Search();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedCity = _viewModel.SelectedCity;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}