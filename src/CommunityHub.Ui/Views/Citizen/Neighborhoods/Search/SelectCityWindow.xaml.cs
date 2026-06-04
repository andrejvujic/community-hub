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
            _viewModel.ConfirmRequested += (_, _) =>
            {
                SelectedCity = _viewModel.SelectedCity;
                DialogResult = true;
            };
            _viewModel.CancelRequested += (_, _) =>
            {
                DialogResult = false;
            };
            _viewModel.CloseRequested += (_, _) => Close();
            DataContext = _viewModel;

            Loaded += (s, e) => CitiesList.Focus();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
