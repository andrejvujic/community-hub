using System.Windows;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ProviderReviewWindow : Window
    {
        private readonly ProviderReviewWindowViewModel _viewModel;

        public ProviderReviewDto? SavedReview => _viewModel.SavedReview;

        public ProviderReviewWindow(BookedJobWithReviewDto bookedJobWithReviewDto, UserDto coordinator)
        {
            InitializeComponent();
            _viewModel = new ProviderReviewWindowViewModel(bookedJobWithReviewDto, coordinator);
            DataContext = _viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var (success, error) = _viewModel.TrySubmit();
            if (!success)
            {
                MessageBox.Show(error, "Nevažeći unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}