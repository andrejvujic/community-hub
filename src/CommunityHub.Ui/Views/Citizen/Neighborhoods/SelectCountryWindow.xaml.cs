using System.Windows;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SelectCountryWindow : Window
    {
        private readonly SelectCountryViewModel _viewModel;
        public Country? SelectedCountry { get; private set; }

        public SelectCountryWindow(Country? initial = null)
        {
            InitializeComponent();
            _viewModel = new SelectCountryViewModel(initial);
            DataContext = _viewModel;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Search();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedCountry = _viewModel.SelectedCountry;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}