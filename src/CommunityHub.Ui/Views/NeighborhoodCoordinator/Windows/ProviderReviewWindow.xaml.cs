using System.Windows;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ProviderReviewWindow : Window
    {
        private readonly ProviderReviewWindowViewModel _viewModel;

        public ProviderReviewWindow(BookedJobDto bookedJob, UserDto coordinator)
        {
            InitializeComponent();

            _viewModel = new ProviderReviewWindowViewModel(bookedJob, coordinator);
            DataContext = _viewModel;
        }
    }
}