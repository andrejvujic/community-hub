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
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class ServiceCenterTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly JobListingService _jobListingService = new();
        private readonly BookedJobService _bookedJobService = new();
        private readonly ProviderReviewService _providerReviewService = new();
        private readonly UserService _userService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<JobListingDto> JobListings { get; } = new();
        public ObservableCollection<BookedJobDto> ScheduledServices { get; } = new();

        public UserDto? Coordinator { get; private set; }

        public RelayCommand PrevImageCommand { get; }
        public RelayCommand NextImageCommand { get; }
        public RelayCommand FindSlotCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand EditReviewCommand { get; }

        private string _durationHoursInput = "1";
        public string DurationHoursInput
        {
            get => _durationHoursInput;
            set 
            { 
                _durationHoursInput = value; 
                OnPropertyChanged(); 
            }
        }

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

        private BookedJobDto? _selectedScheduledService;
        public BookedJobDto? SelectedScheduledService
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

        public Visibility NeighborhoodDetailsVisibility => SelectedNeighborhood != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility JobDetailsVisibility => SelectedListing != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DurationPanelVisibility => IsPerHour ? Visibility.Visible : Visibility.Collapsed;

        public bool IsPerHour => SelectedListing?.BillingType == BillingType.PerHour;

        public string ServicesTitle => SelectedNeighborhood is null ? string.Empty :
            $"Dostupne servisne usluge za kvart {SelectedNeighborhood.Name} (lokacija {SelectedNeighborhood.City.Name}, {SelectedNeighborhood.Country.Name})";

        public string PriceText => SelectedListing?.CurrentPriceEntry is not null
            ? $"Trenutna cena: {SelectedListing.CurrentPriceEntry.Amount:N2} RSD"
            : "Cena nije dostupna";

        public string BillingTypeText => IsPerHour ? "Plaćanje po satu" : "Plaćanje po usluzi";

        public BitmapImage? CurrentImage
        {
            get
            {
                if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0) return null;
                try
                {
                    return new BitmapImage(new Uri(SelectedListing.WorkSamples[_currentImageIndex].Path, UriKind.RelativeOrAbsolute));
                }
                catch { return null; }
            }
        }

        public bool CanReview => SelectedScheduledService != null && _bookedJobService.CanReview(SelectedScheduledService);
        public bool CanEditReview => SelectedScheduledService != null && _bookedJobService.CanEditReview(SelectedScheduledService);

        public ServiceCenterTabViewModel()
        {
            PrevImageCommand = new RelayCommand(PreviousImage);
            NextImageCommand = new RelayCommand(NextImage);
            FindSlotCommand = new RelayCommand(ExecuteFindSlot);
            ReviewCommand = new RelayCommand(ExecuteReview);
            EditReviewCommand = new RelayCommand(ExecuteEditReview);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadCoordinator();
            LoadNeighborhoods();
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        private void ExecuteFindSlot()
        {
            var window = GetActiveWindow();

            if (!TryValidateDuration(window, out int? durationHours))
                return;

            try
            {
                ProcessBooking(window, durationHours);
            }
            catch (ArgumentException ex)
            {
                ShowError(window, "Sistemska greška", ex.Message);
            }
        }

        private bool TryValidateDuration(Window window, out int? durationHours)
        {
            durationHours = null;
            if (!IsPerHour) return true;

            if (!int.TryParse(DurationHoursInput?.Trim(), out int hours) || hours <= 0)
                return ShowError(window, "Nevažeći unos", "Molimo unesite ispravno trajanje usluge u satima.");

            if (hours > 14)
                return ShowError(window, "Greška", "Nije moguće rezervisati termin duži od 14h.");

            durationHours = hours;
            return true;
        }

        private void ProcessBooking(Window window, int? durationHours)
        {
            var slot = _bookedJobService.FindFirstAvailableSlot(SelectedListing!, durationHours);
            if (slot is null)
            {
                ShowError(window, "Nema slobodnih termina", "Nije pronađen slobodan termin u narednih 60 dana.");
                return;
            }

            var confirmWindow = new ServiceBookingConfirmationWindow(SelectedListing!, slot) { Owner = window };
            if (confirmWindow.ShowDialog() == true)
            {
                var booked = _bookedJobService.Book(SelectedListing!, SelectedNeighborhood!, slot);
                ScheduledServices.Add(booked);
                new InfoDialog("Zakazano", $"Termin je uspešno zakazan za {booked.ScheduledDate:dd.MM.yyyy.}.", window).ShowDialog();
            }
        }

        private bool ShowError(Window win, string title, string msg)
        {
            new InfoDialog(title, msg, win).ShowDialog();
            return false;
        }

        private void ExecuteReview()
        {
            if (!CanReview)
            {
                new InfoDialog("Ocenjivanje nije moguće", "Ova usluga ne može biti ocenjena.", GetActiveWindow()).ShowDialog();
                return;
            }
            OpenReviewWindow(SelectedScheduledService!);
        }

        private void ExecuteEditReview()
        {
            if (!CanEditReview)
            {
                new InfoDialog("Izmena nije dozvoljena", "Recenzija se može menjati samo u roku od 3 dana od kreiranja.", GetActiveWindow()).ShowDialog();
                return;
            }
            OpenReviewWindow(SelectedScheduledService!);
        }

        private void OpenReviewWindow(BookedJobDto selected)
        {
            if (Coordinator is null) return;
            var window = new ProviderReviewWindow(selected, Coordinator) 
            { 
                Owner = GetActiveWindow() 
            };

            if (window.ShowDialog() == true)
            {
                LoadScheduledServices();
            }
        }

        public bool LoadCoordinator() 
        { 
            Coordinator = _userService.Get(_coordinatorId); 
            return Coordinator != null; 
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            foreach (var n in _neighborhoodService.GetByCoordinator(_coordinatorId)) Neighborhoods.Add(n);
        }

        private void LoadJobListings()
        {
            JobListings.Clear();
            SelectedListing = null;
            if (SelectedNeighborhood is null) return;
            foreach (var l in _jobListingService.GetByCityAndCountry(SelectedNeighborhood.City.Id, SelectedNeighborhood.Country.Id))
                JobListings.Add(l);
        }

        public void LoadScheduledServices()
        {

            _providerReviewService.GetAllByNeighborhood(SelectedNeighborhood!.Id);
            ScheduledServices.Clear();
            if (SelectedNeighborhood is null) return;

            var jobs = _bookedJobService.GetByNeighborhood(SelectedNeighborhood.Id);
            foreach (var job in jobs)
            {
                ScheduledServices.Add(job);
            }
        }

        private void PreviousImage()
        {
            if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex - 1 + SelectedListing.WorkSamples.Count) % SelectedListing.WorkSamples.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        private void NextImage()
        {
            if (SelectedListing is null || SelectedListing.WorkSamples.Count == 0) return;
            _currentImageIndex = (_currentImageIndex + 1) % SelectedListing.WorkSamples.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}