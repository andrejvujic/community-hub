using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.LocalFacilities
{
    public class ReservationStatisticsViewModel : INotifyPropertyChanged
    {
        private bool _isMonthView = true;
        private DateTime _currentDate = DateTime.Now;
        private long _neighborhoodId;
        private readonly FacilityReservationService _facilityReservationService = new();
        private static readonly CultureInfo SerbianCulture = new CultureInfo("sr-Latn-RS");
        private ReservationStatisticItem? _selectedStatistic;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }
        public ICommand TogglePeriodCommand { get; }
        public ICommand PreviousPeriodCommand { get; }
        public ICommand NextPeriodCommand { get; }

        public ObservableCollection<ReservationStatisticItem> Statistics { get; }
        private int _totalReservations;

        public int TotalReservations
        {
            get => _totalReservations;
            set
            {
                if (_totalReservations != value)
                {
                    _totalReservations = value;
                    OnPropertyChanged(nameof(TotalReservations));
                    OnPropertyChanged(nameof(TotalReservationsText));
                }
            }
        }

        public ReservationStatisticItem? SelectedStatistic
        {
            get => _selectedStatistic;
            set
            {
                if (_selectedStatistic != value)
                {
                    _selectedStatistic = value;
                    OnPropertyChanged(nameof(SelectedStatistic));
                }
            }
        }

        public string TotalReservationsText => $"Ukupne posete: {TotalReservations}";

        public bool IsMonthView
        {
            get => _isMonthView;
            set
            {
                if (_isMonthView != value)
                {
                    _isMonthView = value;
                    OnPropertyChanged(nameof(IsMonthView));
                    OnPropertyChanged(nameof(IsYearView));
                    OnPropertyChanged(nameof(PeriodText));
                    LoadData();
                }
            }
        }

        public bool IsYearView
        {
            get => !_isMonthView;
            set => IsMonthView = !value;
        }

        public string PeriodText
        {
            get
            {
                if (IsMonthView)
                {
                    return _currentDate.ToString("MMMM yyyy", SerbianCulture);
                }
                else
                {
                    return _currentDate.Year.ToString();
                }
            }
        }

        public ReservationStatisticsViewModel(long neighborhoodId)
        {
            _neighborhoodId = neighborhoodId;
            Statistics = new ObservableCollection<ReservationStatisticItem>();

            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            TogglePeriodCommand = new RelayCommand<object>(_ => IsMonthView = !IsMonthView);
            PreviousPeriodCommand = new RelayCommand<object>(_ => NavigatePrevious());
            NextPeriodCommand = new RelayCommand<object>(_ => NavigateNext());

            LoadData();
        }

        private void NavigatePrevious()
        {
            if (IsMonthView)
            {
                _currentDate = _currentDate.AddMonths(-1);
            }
            else
            {
                _currentDate = _currentDate.AddYears(-1);
            }
            OnPropertyChanged(nameof(PeriodText));
            LoadData();
        }

        private void NavigateNext()
        {
            if (IsMonthView)
            {
                _currentDate = _currentDate.AddMonths(1);
            }
            else
            {
                _currentDate = _currentDate.AddYears(1);
            }
            OnPropertyChanged(nameof(PeriodText));
            LoadData();
        }

        private void LoadData()
        {
            Statistics.Clear();
            TotalReservations = 0;

            var allReservations = _facilityReservationService.GetByNeighborhoodId(_neighborhoodId);
            var filteredReservations = allReservations.Where(r => IsInPeriod(r.StartDatetime)).ToList();

            PopulateReservations();

            SelectedStatistic = Statistics.FirstOrDefault();
        }

        public void PopulateReservations()
        {
            var allReservations = _facilityReservationService.GetByNeighborhoodId(_neighborhoodId);
            var filteredReservations = allReservations.Where(r => IsInPeriod(r.StartDatetime)).ToList();

            var statisticsByFacility = filteredReservations
                .GroupBy(r => r.Facility.Id)
                .Select(g => new ReservationStatisticItem
                {
                    FacilityName = g.First().Facility.Name,
                    VisitCount = g.Count()
                })
                .OrderBy(s => s.FacilityName)
                .ToList();

            foreach (var stat in statisticsByFacility)
            {
                Statistics.Add(stat);
                TotalReservations += stat.VisitCount;
            }
        }

        private bool IsInPeriod(DateTime date)
        {
            if (IsMonthView)
            {
                return date.Year == _currentDate.Year && date.Month == _currentDate.Month;
            }
            else
            {
                return date.Year == _currentDate.Year;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ReservationStatisticItem
    {
        public string FacilityName { get; set; }
        public int VisitCount { get; set; }
    }
}
