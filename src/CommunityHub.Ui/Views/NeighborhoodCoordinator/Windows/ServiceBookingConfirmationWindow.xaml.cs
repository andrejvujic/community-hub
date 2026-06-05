using System.Windows;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ServiceBookingConfirmationWindow : Window
    {
        private readonly BookingConfirmationWindowViewModel _viewModel;

        public ServiceBookingConfirmationWindow(JobListingDto listing, AvailableJobSlotDto slot)
        {
            InitializeComponent();
            _viewModel = new BookingConfirmationWindowViewModel(listing, slot);
            DataContext = _viewModel;
        }
    }
}