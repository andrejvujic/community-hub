using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.JobListingBookings;
using CommunityHub.Application.Services.JobListings;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class ServiceCenterTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly JobListingService _jobListingService = new();
        private readonly BookedJobService _bookedJobService = new();
        private readonly ProviderReviewService _providerReviewService = new();
        private readonly UserService _userService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<JobListingDto> JobListings { get; } = new();
        public ObservableCollection<BookedJobWithReviewDto> ScheduledServices { get; } = new();

        public UserDto? Coordinator { get; private set; }


        private NeighborhoodDto? _selectedNeighborhood;
        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ServicesTitle));
                OnPropertyChanged(nameof(NeighborhoodDetailsVisibility));
                LoadJobListings();
                LoadScheduledServices();
            }
        }


        private JobListingDto? _selectedListing;
        public JobListingDto? SelectedListing
        {
            get => _selectedListing;
            set
            {
                if (_selectedListing == value) return;
                _selectedListing = value;
                _currentImageIndex = 0;
                OnPropertyChanged();
                OnPropertyChanged(nameof(JobDetailsVisibility));
                OnPropertyChanged(nameof(DurationPanelVisibility));
                OnPropertyChanged(nameof(IsPerHour));
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(PriceText));
                OnPropertyChanged(nameof(BillingTypeText));
            }
        }


        private BookedJobWithReviewDto? _selectedScheduledService;
        public BookedJobWithReviewDto? SelectedScheduledService
        {
            get => _selectedScheduledService;
            set
            {
                if (_selectedScheduledService == value) return;
                _selectedScheduledService = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanReview));
                OnPropertyChanged(nameof(CanEditReview));
            }
        }

        private int _currentImageIndex;


        public Visibility NeighborhoodDetailsVisibility =>
            SelectedNeighborhood != null ? Visibility.Visible : Visibility.Collapsed;

        public Visibility JobDetailsVisibility =>
            SelectedListing != null ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DurationPanelVisibility =>
            IsPerHour ? Visibility.Visible : Visibility.Collapsed;


        public bool IsPerHour => SelectedListing?.BillingType == BillingType.PerHour;

        public string ServicesTitle => SelectedNeighborhood is null ? string.Empty :
            $"Dostupne servisne usluge za kvart {SelectedNeighborhood.Name} " +
            $"(lokacija {SelectedNeighborhood.City.Name}, {SelectedNeighborhood.Country.Name})";

        public string PriceText => SelectedListing?.CurrentPriceEntry is not null
            ? $"Trenutna cena: {SelectedListing.CurrentPriceEntry.Amount:N2} RSD"
            : "Cena nije dostupna";

        public string BillingTypeText => IsPerHour ? "Plaćanje po satu" : "Plaćanje po usluzi";

        public BitmapImage? CurrentImage
        {
            get
            {
                if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0)
                    return null;
                try
                {
                    var path = SelectedListing.WorkSamples[_currentImageIndex].Path;
                    return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                }
                catch { return null; }
            }
        }

        public bool CanReview => SelectedScheduledService?.CanReview ?? false;
        public bool CanEditReview => SelectedScheduledService?.CanEditReview ?? false;


        public ServiceCenterTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }


        public bool LoadCoordinator()
        {
            Coordinator = _userService.Get(_coordinatorId);
            return Coordinator != null;
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            foreach (var n in _neighborhoodService.GetByCoordinator(_coordinatorId))
                Neighborhoods.Add(n);
        }

        private void LoadJobListings()
        {
            JobListings.Clear();
            SelectedListing = null;
            if (SelectedNeighborhood is null) return;

            foreach (var l in _jobListingService.GetByCityAndCountry(
                SelectedNeighborhood.City.Id, SelectedNeighborhood.Country.Id))
                JobListings.Add(l);
        }

        public void LoadScheduledServices()
        {
            ScheduledServices.Clear();
            if (SelectedNeighborhood is null) return;

            var jobs = _bookedJobService.GetByNeighborhood(SelectedNeighborhood.Id);
            var reviews = _providerReviewService.GetAllByNeighborhood(SelectedNeighborhood.Id);

            foreach (var job in jobs)
            {
                reviews.TryGetValue(job.Id, out var review);
                ScheduledServices.Add(new BookedJobWithReviewDto(job, review));
            }
        }

        public void PreviousImage()
        {
            if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex - 1 + SelectedListing.WorkSamples.Count)
                                  % SelectedListing.WorkSamples.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        public void NextImage()
        {
            if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex + 1) % SelectedListing.WorkSamples.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        public AvailableJobSlotDto? FindFirstAvailableSlot(int? durationHours)
        {
            if (SelectedListing is null) return null;
            return _bookedJobService.FindFirstAvailableSlot(SelectedListing, durationHours);
        }

        public BookedJobDto? Book(AvailableJobSlotDto availableJobSlotDto)
        {
            if (SelectedListing is null || SelectedNeighborhood is null) return null;
            var booked = _bookedJobService.Book(SelectedListing, SelectedNeighborhood, availableJobSlotDto);
            ScheduledServices.Add(new BookedJobWithReviewDto(booked, review: null));
            return booked;
        }

        public void UpdateReview(BookedJobWithReviewDto bookedJobWithReviewDto, ProviderReviewDto providerReview)
        {
            bookedJobWithReviewDto.SetReview(providerReview);

            int index = ScheduledServices.IndexOf(bookedJobWithReviewDto);
            if (index >= 0)
            {
                ScheduledServices.RemoveAt(index);
                ScheduledServices.Insert(index, bookedJobWithReviewDto);
            }

            OnPropertyChanged(nameof(CanReview));
            OnPropertyChanged(nameof(CanEditReview));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
