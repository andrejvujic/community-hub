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

            if (_viewModel != null)
            {
                _viewModel.ConfirmRequested += (_, _) => { DialogResult = true; };
                _viewModel.CancelRequested += (_, _) => { DialogResult = false; };
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
