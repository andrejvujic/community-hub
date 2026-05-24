using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SelectCountryWindow : Window
    {
        private readonly SelectCountryViewModel _viewModel;
        public CountryDto? SelectedCountry { get; private set; }

        public SelectCountryWindow(CountryDto? initial = null)
        {
            InitializeComponent();
            _viewModel = new SelectCountryViewModel(initial);
            _viewModel.ConfirmRequested += (_, _) =>
            {
                SelectedCountry = _viewModel.SelectedCountry;
                DialogResult = true;
            };
            _viewModel.CancelRequested += (_, _) =>
            {
                DialogResult = false;
            };
            _viewModel.CloseRequested += (_, _) => Close();
            DataContext = _viewModel;

            Loaded += (s, e) => CountriesList.Focus();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
