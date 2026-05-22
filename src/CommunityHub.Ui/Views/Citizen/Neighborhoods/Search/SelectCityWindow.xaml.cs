using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SelectCityWindow : Window
    {
        private readonly SelectCityViewModel _viewModel;
        public CityDto? SelectedCity { get; private set; }

        public SelectCityWindow(CountryDto? selectedCountry = null)
        {
            InitializeComponent();
            _viewModel = new SelectCityViewModel(selectedCountry);
            DataContext = _viewModel;
            Loaded += (s, e) => CitiesList.Focus();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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
