using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalReportViewModel : BaseViewModel
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");

        public CommonRoomRentalReportViewModel(
            long userId,
            long buildingId,
            IMainNavigationConsumer navigationConsumer) : base(navigationConsumer)
        {
            UserId = userId;
            BuildingId = buildingId;
            GenerateReportCommand = new(GenerateReport, CanGenerateReport);
        }

        public long UserId { get; }
        public long BuildingId { get; }
        public RelayCommand GenerateReportCommand { get; }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(SelectedDateText));
                GenerateReportCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedDateText
            => SelectedDate is null
                ? "Izaberite..."
                : SelectedDate.Value.ToString("d. MMMM yyyy.", _culture);

        private void GenerateReport()
        {
        }

        private bool CanGenerateReport()
            => SelectedDate.HasValue;
    }
}
