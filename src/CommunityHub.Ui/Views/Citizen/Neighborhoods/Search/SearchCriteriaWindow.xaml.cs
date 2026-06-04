using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SearchCriteriaWindow : Window
    {
        private readonly SearchCriteriaViewModel _viewModel;
        public NeighborhoodSearchParameters? Result { get; private set; }

        public SearchCriteriaWindow(NeighborhoodSearchParameters? initial = null)
        {
            InitializeComponent();
            _viewModel = new SearchCriteriaViewModel(initial);
            _viewModel.ConfirmRequested += (_, _) =>
            {
                Result = _viewModel.BuildSearchParameters();
                DialogResult = true;
            };
            _viewModel.CancelRequested += (_, _) =>
            {
                DialogResult = false;
            };
            _viewModel.CloseRequested += (_, _) => Close();
            _viewModel.ClearRequested += (_, _) =>
            {
                _viewModel.Clear();
                Result = null;
                DialogResult = true;
            };
            DataContext = _viewModel;

            Loaded += (_, _) => NameTextBox.Focus();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void SelectCountryButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectCountryWindow(_viewModel.SelectedCountry);
            if (window.ShowDialog() == true)
            {
                _viewModel.SelectedCountry = window.SelectedCountry;
            }
        }

        private void SelectCityButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectCityWindow(_viewModel.SelectedCountry);
            if (window.ShowDialog() == true)
            {
                _viewModel.SelectedCity = window.SelectedCity;
            }
        }
    }
}
