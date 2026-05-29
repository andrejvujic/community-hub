using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.LocalFacilities
{
    public class ReservationStatisticsViewModel : INotifyPropertyChanged
    {
        private bool _isMonthView = true;
        private DateTime _currentDate = DateTime.Now;
        private static readonly CultureInfo SerbianCulture = new CultureInfo("sr-Latn-RS");

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }
        public ICommand TogglePeriodCommand { get; }
        public ICommand PreviousPeriodCommand { get; }
        public ICommand NextPeriodCommand { get; }

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

        public ReservationStatisticsViewModel()
        {
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            TogglePeriodCommand = new RelayCommand<object>(_ => IsMonthView = !IsMonthView);
            PreviousPeriodCommand = new RelayCommand<object>(_ => NavigatePrevious());
            NextPeriodCommand = new RelayCommand<object>(_ => NavigateNext());
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
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
