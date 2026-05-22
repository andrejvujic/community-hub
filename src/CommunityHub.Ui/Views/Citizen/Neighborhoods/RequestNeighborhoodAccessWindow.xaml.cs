using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class RequestNeighborhoodAccessWindow : Window
    {
        private RequestNeighborhoodAccessViewModel? _viewModel;

        public bool Confirmed => _viewModel?.Confirmed ?? false;

        public RequestNeighborhoodAccessWindow(string neighborhoodName)
        {
            _viewModel = new RequestNeighborhoodAccessViewModel(neighborhoodName);
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Confirmed = false;
            DialogResult = false;
            Close();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
