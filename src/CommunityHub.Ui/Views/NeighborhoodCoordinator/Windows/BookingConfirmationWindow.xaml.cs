using System.Windows;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class BookingConfirmationWindow : Window
    {
        private readonly BookingConfirmationWindowViewModel _viewModel;
        public bool Confirmed { get; private set; }

        public BookingConfirmationWindow(JobListingDto listing, AvailableJobSlotDto slot)
        {
            InitializeComponent();
            _viewModel = new BookingConfirmationWindowViewModel(listing, slot);
            DataContext = _viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
            Close();
        }
    }
}