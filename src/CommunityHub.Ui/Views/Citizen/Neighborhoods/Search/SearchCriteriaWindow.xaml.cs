using System.Windows;
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
            DataContext = _viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Result = _viewModel.BuildSearchParameters();
            DialogResult = true;
            Close();
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Clear();
            Result = null;
            DialogResult = true;
        }
    }
}
