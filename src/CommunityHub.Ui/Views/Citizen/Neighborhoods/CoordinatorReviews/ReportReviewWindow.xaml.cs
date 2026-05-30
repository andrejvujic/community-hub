using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods.CoordinatorReviews
{
    public partial class ReportReviewWindow : Window
    {
        private ReportReviewViewModel? _viewModel;

        public ReportReviewWindow(string reviewerName)
        {
            _viewModel = new ReportReviewViewModel(reviewerName);
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
            {
                DragMove();
            }
        }
    }
}
