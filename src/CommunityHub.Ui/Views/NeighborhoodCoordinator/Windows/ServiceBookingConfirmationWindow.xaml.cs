using System.Windows;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ServiceBookingConfirmationWindow : Window
    {
        private readonly ServiceBookingConfirmationWindowViewModel _viewModel;

        public ServiceBookingConfirmationWindow(JobListingDto listing, AvailableJobSlotDto slot)
        {
            InitializeComponent();
            _viewModel = new ServiceBookingConfirmationWindowViewModel(listing, slot);
            DataContext = _viewModel;
        }
    }
}