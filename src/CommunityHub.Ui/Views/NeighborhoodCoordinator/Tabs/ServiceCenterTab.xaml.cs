using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class ServiceCenterTab : UserControl
    {
        private ServiceCenterTabViewModel _viewModel = null!;

        public ServiceCenterTab() => InitializeComponent();

        public void Initialize(long coordinatorId)
        {
            _viewModel = new ServiceCenterTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadCoordinator();
            _viewModel.LoadNeighborhoods();
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedNeighborhood = NeighborhoodsListBox.SelectedItem as NeighborhoodDto;

        private void JobListingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedListing = JobListingsListBox.SelectedItem as JobListingDto;

        private void PrevImageButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.PreviousImage();

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.NextImage();

        private void FindSlotButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDurationHours(out int? durationHours)) return;

            var slot = _viewModel.FindFirstAvailableSlot(durationHours);
            if (slot is null)
            {
                MessageBox.Show("Nije pronađen slobodan termin u narednih 60 dana.",
                    "Nema slobodnih termina", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            BookIfConfirmed(slot);
        }

        private bool TryGetDurationHours(out int? durationHours)
        {
            durationHours = null;
            if (!_viewModel.IsPerHour) return true;

            if (int.TryParse(DurationHoursTextBox.Text.Trim(), out int hours) && hours > 0)
            {
                durationHours = hours;
                return true;
            }
            MessageBox.Show("Molimo unesite ispravno trajanje usluge u satima.",
                "Nevažeći unos", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void BookIfConfirmed(AvailableJobSlotDto availableJobSlotDto)
        {
            /*
            var window = new BookingConfirmationWindow(_viewModel.SelectedListing!, availableJobSlotDto)
            { Owner = Window.GetWindow(this) };

            if (window.ShowDialog() != true) return;

            var booked = _viewModel.Book(availableJobSlotDto);
            MessageBox.Show($"Termin je uspešno zakazan za {booked!.ScheduledDate:dd.MM.yyyy.}.",
                "Zakazano", MessageBoxButton.OK, MessageBoxImage.Information);
            */
        }

        private void ScheduledServicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedScheduledService =
                ScheduledServicesDataGrid.SelectedItem as BookedJobWithReviewDto;

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.CanReview)
            {
                MessageBox.Show("Ova usluga ne može biti ocenjena.",
                    "Ocenjivanje nije moguće", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            OpenReviewWindow(_viewModel.SelectedScheduledService!);
        }

        private void EditReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.CanEditReview)
            {
                MessageBox.Show("Recenzija se može menjati samo u roku od 3 dana od kreiranja.",
                    "Izmena nije dozvoljena", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            OpenReviewWindow(_viewModel.SelectedScheduledService!);
        }

        private void OpenReviewWindow(BookedJobWithReviewDto selected)
        {
            /*
            if (_viewModel.Coordinator is null) return;

            var window = new ProviderReviewWindow(selected, _viewModel.Coordinator)
            { 
                Owner = Window.GetWindow(this) 
            };

            if (window.ShowDialog() == true)
                _viewModel.UpdateReview(selected, window.SavedReview!);
            */
        }
    }
}
