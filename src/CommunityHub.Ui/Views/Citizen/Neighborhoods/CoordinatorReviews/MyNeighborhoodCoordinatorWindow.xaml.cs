using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods.CoordinatorReviews
{
    public partial class MyNeighborhoodCoordinatorWindow : Window
    {
        private MyNeighborhoodCoordinatorViewModel? _viewModel;

        public MyNeighborhoodCoordinatorWindow(long userId)
        {
            InitializeComponent();
            _viewModel = new MyNeighborhoodCoordinatorViewModel(userId);
            _viewModel.CloseRequested += (_, _) => Close();
            _viewModel.WriteNewReviewRequested += (_, _) => HandleWriteNewReview();
            _viewModel.ReportReviewRequested += (_, _) => HandleReportReview();
            DataContext = _viewModel;
        }

        private void HandleWriteNewReview()
        {
        }

        private void HandleReportReview()
        {
            if (_viewModel?.SelectedReview is not CoordinatorReviewDto review) return;
            var reportReviewWindow = new ReportReviewWindow($"{review.Reviewer.Name} {review.Reviewer.Surname}");

            if (reportReviewWindow.ShowDialog() == true)
            {
                _viewModel.ExecuteReport(review.Id);
            }
        }

        private void Header_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
