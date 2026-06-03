using CommunityHub.Application.Domain.CommonRooms.Services;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalReportViewModel : BaseViewModel
    {
        private readonly RentalRequestsReportGenerator _reportGenerator = new();
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        public CommonRoomRentalReportViewModel(
            long userId,
            long buildingId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            UserId = userId;
            BuildingId = buildingId;
            GenerateReportCommand = new(GenerateReport, CanGenerateReport);
        }

        public long UserId { get; }
        public long BuildingId { get; }
        public RelayCommand GenerateReportCommand { get; }

        private DateTime? _selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate == value) return;
                _selectedStartDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedStartDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));
                OnPropertyChanged(nameof(MinEndDate));
                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                GenerateReportCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedStartDateText
            => SelectedStartDate is null
                ? "Izaberite..."
                : SelectedStartDate.Value.ToString("d. MMMM", _culture);

        private DateTime? _selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate == value) return;
                _selectedEndDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedEndDate));
                OnPropertyChanged(nameof(SelectedEndDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));
                OnPropertyChanged(nameof(MaxStartDate));
                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                GenerateReportCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedEndDateText
            => SelectedEndDate is null
                ? "Izaberite..."
                : SelectedEndDate.Value.ToString("d. MMMM", _culture);

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                return $"{SelectedStartDateText} — {SelectedEndDateText}";
            }
        }

        public bool IsSelectedDateRangeValid
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return false;
                if (SelectedStartDate.Value < _today) return false;
                if (SelectedEndDate.Value < SelectedStartDate.Value) return false;

                return true;
            }
        }

        public DateTime? MaxStartDate => SelectedEndDate;
        public DateTime MinEndDate => SelectedStartDate ?? _today;

        private void GenerateReport()
        {
            if (SelectedStartDate is null || SelectedEndDate is null) return;

            var rentalRequestReports = _reportGenerator.GenerateForBuilding(
                BuildingId, 
                SelectedStartDate.Value, 
                SelectedEndDate.Value
            );
        }

        private bool CanGenerateReport()
            => IsSelectedDateRangeValid;
    }
}
