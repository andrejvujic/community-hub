using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods.CoordinatorReviews
{
    public partial class WriteNewReviewWindow : Window
    {
        private WriteNewReviewViewModel? _viewModel;

        public WriteNewReviewWindow(string neighborhoodName, string coordinatorName)
        {
            _viewModel = new WriteNewReviewViewModel(neighborhoodName, coordinatorName);
            DataContext = _viewModel;
            InitializeComponent();

            if (_viewModel != null)
            {
                _viewModel.ConfirmRequested += (_, _) => { DialogResult = true; };
                _viewModel.CancelRequested += (_, _) => { DialogResult = false; };
            }

            Loaded += (_, _) => RatingTextBox.Focus();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
