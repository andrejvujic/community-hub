using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                new InfoDialog("Nema slobodnih termina", "Nije pronađen slobodan termin u narednih 60 dana.", Window.GetWindow(this)).ShowDialog();
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
            new InfoDialog("Nevažeći unos", "Molimo unesite ispravno trajanje usluge u satima.", Window.GetWindow(this)).ShowDialog();
            return false;
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }

        private void BookIfConfirmed(AvailableJobSlotDto availableJobSlotDto)
        {
            var window = new ServiceBookingConfirmationWindow(_viewModel.SelectedListing!, availableJobSlotDto)
            { Owner = Window.GetWindow(this) };

            if (window.ShowDialog() != true) return;

            var booked = _viewModel.Book(availableJobSlotDto);
            new InfoDialog("Zakazano", $"Termin je uspešno zakazan za {booked!.ScheduledDate:dd.MM.yyyy.}.", Window.GetWindow(this)).ShowDialog();
        }

        private void ScheduledServicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => _viewModel.SelectedScheduledService =
                ScheduledServicesDataGrid.SelectedItem as BookedJobWithReviewDto;

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.CanReview)
            {
                new InfoDialog("Ocenjivanje nije moguće", "Ova usluga ne može biti ocenjena.", Window.GetWindow(this)).ShowDialog();
                return;
            }
            OpenReviewWindow(_viewModel.SelectedScheduledService!);
        }

        private void EditReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.CanEditReview)
            {
                new InfoDialog("Izmena nije dozvoljena", "Recenzija se može menjati samo u roku od 3 dana od kreiranja.", Window.GetWindow(this)).ShowDialog();
                return;
            }
            OpenReviewWindow(_viewModel.SelectedScheduledService!);
        }

        private void OpenReviewWindow(BookedJobWithReviewDto selected)
        {
            if (_viewModel.Coordinator is null) return;

            var window = new ProviderReviewWindow(selected, _viewModel.Coordinator)
            { 
                Owner = Window.GetWindow(this) 
            };

            if (window.ShowDialog() == true)
                _viewModel.UpdateReview(selected, window.SavedReview!);
        }
    }
}
