using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Services;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for ServiceCenterTab.xaml
    /// </summary>
    public partial class ServiceCenterTab : UserControl
    {
        private long _coordinatorId;
        private Neighborhood? _selectedNeighborhood;
        private JobListing? _selectedListing;

        private int _currentImageIndex;

        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly JobBookingService _jobBookingService = new(
            new JobListingRepository(),
            new BookedJobRepository()
        );

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<JobListing> JobListings { get; set; } = new();

        public ServiceCenterTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsListBox.SelectedItem as Neighborhood;
            if (_selectedNeighborhood is null) return;

            JobListings.Clear();
            HideJobDetails();

            ServicesTitleTextBlock.Text =
                $"Dostupne servisne usluge za kvart {_selectedNeighborhood.Name} " +
                $"(lokacija {_selectedNeighborhood.City.Name}, {_selectedNeighborhood.Country.Name})";

            var jobListings = _jobBookingService.GetJobListingsForLocation(_selectedNeighborhood.City.Id, _selectedNeighborhood.Country.Id);

            foreach (var jobListing in jobListings)
                JobListings.Add(jobListing);

            NeighborhoodServiceDetails.Visibility = Visibility.Visible;
        }

        private void JobListingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedListing = JobListingsListBox.SelectedItem as JobListing;
            if (_selectedListing is null)
            {
                HideJobDetails();
                return;
            }

            ShowJobDetails(_selectedListing);
        }

        private void ShowJobDetails(JobListing listing)
        {
            JobTitleTextBlock.Text = $"Usluga: {listing.ServiceName}";
            JobProviderTextBlock.Text = $"Serviser: {listing.Provider.Name} {listing.Provider.Surname}";

            JobDescriptionTextBlock.Text = listing.Description;

            JobPriceTextBlock.Text = listing.CurrentPriceEntry is not null
                ? $"Trenutna cena: {listing.CurrentPriceEntry.Amount:N2} RSD"
                : "Cena nije dostupna";

            JobBillingTypeTextBlock.Text = listing.BillingType == BillingType.PerHour 
                ? "Plaćanje po satu" 
                : "Plaćanje po usluzi";

            DurationPanel.Visibility = listing.BillingType == BillingType.PerHour
                ? Visibility.Visible
                : Visibility.Collapsed;

            _currentImageIndex = 0;
            ShowCurrentImage(listing);

            JobDetailsGrid.Visibility = Visibility.Visible;
        }

        private void HideJobDetails()
        {
            JobDetailsGrid.Visibility = Visibility.Collapsed;
        }


        private void PrevImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedListing is null || _selectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex - 1 + _selectedListing.WorkSamples.Count)
                                  % _selectedListing.WorkSamples.Count;
            ShowCurrentImage(_selectedListing);
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedListing is null || _selectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex + 1) % _selectedListing.WorkSamples.Count;
            ShowCurrentImage(_selectedListing);
        }

        private void ShowCurrentImage(JobListing listing)
        {
            if (listing.WorkSamples is null || listing.WorkSamples.Count == 0)
            {
                JobSampleImage.Source = null;
                return;
            }

            var path = listing.WorkSamples[_currentImageIndex].Path;
            try
            {
                JobSampleImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                JobSampleImage.Source = null;
            }
        }

        private void FindSlotButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedListing is null || _selectedNeighborhood is null) return;

            if (!TryGetDurationHours(out int? durationHours)) return;

            var slot = _jobBookingService.FindFirstAvailableSlot(_selectedListing, durationHours);

            if (slot is null)
            {
                ShowNoSlotsMessage();
                return;
            }

            BookIfConfirmed(slot);
        }

        private bool TryGetDurationHours(out int? durationHours)
        {
            durationHours = null;
            if (_selectedListing!.BillingType != BillingType.PerHour) return true;

            if (!int.TryParse(DurationHoursTextBox.Text.Trim(), out int hours) || hours <= 0)
            {
                MessageBox.Show(
                    "Molimo unesite ispravno trajanje usluge u satima.",
                    "Nevažeći unos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            durationHours = hours;
            return true;
        }

        private void ShowNoSlotsMessage()
        {
            MessageBox.Show(
                "Nije pronađen slobodan termin u narednih 365 dana.",
                "Nema slobodnih termina",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BookIfConfirmed(AvailableJobSlot slot)
        {
            var dialog = new BookingConfirmationWindow(_selectedListing!, slot)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true || !dialog.Confirmed) return;

            var booked = _jobBookingService.BookService(_selectedListing!, _selectedNeighborhood!, slot);

            MessageBox.Show(
                $"Termin je uspešno zakazan za {booked.ScheduledDate:dd.MM.yyyy.}.",
                "Zakazano",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
