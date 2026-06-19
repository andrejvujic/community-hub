using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class CityFacilitiesTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly CityFacilityService _facilityService = new();
        private readonly FacilityReservationService _reservationService = new();
        private readonly FacilityVoteService _voteService = new();
        private readonly FacilityReservationsReportService _reportService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<CityFacilityDto> Facilities { get; } = new();
        public ObservableCollection<FacilityReservationDto> Reservations { get; } = new();

        public RelayCommand GenerateReportCommand { get; }
        public RelayCommand ReserveCommand { get; }

        public DateTime ReservationMinDate => DateTime.Today.AddDays(1);

        private string _durationInput = string.Empty;
        public string DurationInput 
        { 
            get => _durationInput; 
            set 
            { 
                _durationInput = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _reservationStartDate;
        public DateTime? ReservationStartDate 
        { 
            get => _reservationStartDate; 
            set 
            { 
                _reservationStartDate = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _reservationEndDate;
        public DateTime? ReservationEndDate 
        { 
            get => _reservationEndDate; 
            set 
            { _reservationEndDate = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _reportStartDate;
        public DateTime? ReportStartDate
        {
            get => _reportStartDate;
            set 
            { 
                _reportStartDate = value; 
                OnPropertyChanged(); 
            }
        }

        private DateTime? _reportEndDate;
        public DateTime? ReportEndDate
        {
            get => _reportEndDate;
            set 
            { 
                _reportEndDate = value; 
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
                OnPropertyChanged(nameof(IsNeighborhoodSelected));
                OnPropertyChanged(nameof(ReservationsTitle));
                LoadFacilities();
                LoadReservations();
            }
        }

        private CityFacilityDto? _selectedFacility;
        public CityFacilityDto? SelectedFacility
        {
            get => _selectedFacility;
            set
            {
                if (_selectedFacility == value) return;
                _selectedFacility = value;
                OnPropertyChanged();
            }
        }

        public bool IsNeighborhoodSelected => SelectedNeighborhood != null;
        public string ReservationsTitle => TranslationSource.Instance.Format("Facilities_ReservationList_Format", SelectedNeighborhood?.Name);

        public CityFacilitiesTabViewModel()
        {
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
            ReserveCommand = new RelayCommand(ExecuteReserve);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            ReservationStartDate = ReservationMinDate;
            ReservationEndDate = ReservationMinDate;
            ReportStartDate = DateTime.Today.AddMonths(-1);
            ReportEndDate = DateTime.Today;
            LoadNeighborhoods();
        }

        public void LoadNeighborhoods()
        {
            var previously = SelectedNeighborhood;
            Neighborhoods.Clear();

            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);

            SelectedNeighborhood = previously != null
                ? Neighborhoods.FirstOrDefault(n => n.Id == previously.Id)
                : null;
        }

        private void LoadFacilities()
        {
            Facilities.Clear();
            SelectedFacility = null;
            if (SelectedNeighborhood is null) return;

            var facilities = _facilityService.GetByCityId(SelectedNeighborhood.City.Id);
            foreach (var f in facilities)
                f.VoteCount = _voteService.GetVoteCountByFacilityAndNeighborhoodId(f.Id, SelectedNeighborhood.Id);

            foreach (var f in facilities.OrderByDescending(f => f.VoteCount))
                Facilities.Add(f);
        }

        private void LoadReservations()
        {
            Reservations.Clear();
            if (SelectedNeighborhood is null) return;

            foreach (var r in _reservationService.GetByNeighborhoodId(SelectedNeighborhood.Id)
                .OrderByDescending(r => r.StartDatetime))
                Reservations.Add(r);
        }

        private void ExecuteReserve()
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                ?? System.Windows.Application.Current.MainWindow;

            if (!TryValidateInputs(window, out int duration, out DateTime start, out DateTime end))
                return;

            try
            {
                var result = FindSlots(duration, start, end);

                if (result is null || (result.Value.SlotInRange is null && result.Value.SlotsOutsideRange.Count == 0))
                {
                    new InfoDialog(TranslationSource.Instance["Facilities_NoSlots_Title"], TranslationSource.Instance["Facilities_NoSlots_Message"], window).ShowDialog();
                    return;
                }

                OpenBookingWindow(window, result.Value);
            }
            catch (ArgumentException ex)
            {
                new InfoDialog(TranslationSource.Instance["Error_Title"], ex.Message, window).ShowDialog();
            }
        }

        private bool TryValidateInputs(Window win, out int duration, out DateTime start, out DateTime end)
        { 
            duration = 0; start = DateTime.MinValue; end = DateTime.MinValue;

            if (SelectedFacility is null) return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectFacility"]);

            if (!int.TryParse(DurationInput?.Trim(), out duration) || duration <= 0)
                return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Facilities_InvalidDuration"]);

            if (duration > 14) return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Facilities_DurationTooLong"]);

            if (ReservationStartDate is null || ReservationEndDate is null) return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectDates"]);

            start = ReservationStartDate.Value;
            end = ReservationEndDate.Value.AddDays(1).AddSeconds(-1);

            return start < end || ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_DateOrder"]);
        }

        private void OpenBookingWindow(Window owner, (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange) data)
        {
            var dialog = new FacilityBookingWindow(SelectedFacility!, SelectedNeighborhood!, data.SlotInRange, data.SlotsOutsideRange, this)
            {
                Owner = owner
            };
            dialog.ShowDialog();
        }

        private bool ShowError(Window win, string title, string msg)
        {
            new InfoDialog(title, msg, win).ShowDialog();
            return false;
        }

        public (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange)? FindSlots(int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            if (SelectedFacility is null) return null;
            return _reservationService.FindSlots(SelectedFacility, durationHours, rangeStart, rangeEnd);
        }

        public FacilityReservationDto Book(AvailableFacilitySlotDto slot)
        {
            var created = _reservationService.Book(SelectedFacility!, SelectedNeighborhood!, slot);
            _voteService.DeleteAllByFacilityAndNeighborhoodId(SelectedFacility!.Id, SelectedNeighborhood!.Id);
            LoadReservations();
            LoadFacilities();
            return created;
        }

        private void ExecuteGenerateReport()
        {
            var window = GetActiveWindow();

            if (!TryValidateReportInputs(window, out DateTime from, out DateTime to))
                return;

            var savePath = ChooseSavePath(from, to, SelectedNeighborhood!.Name);
            if (savePath is null) return;

            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var pdfBytes = _reportService.GenerateBytes(
                    SelectedNeighborhood!.Name,
                    SelectedNeighborhood.Id,
                    from,
                    to);

                SaveReport(pdfBytes, savePath);
                AskToOpenReport(window, savePath);
            }
            catch (Exception ex)
            {
                ShowError(window, "Greška", $"Greška pri generisanju izveštaja: {ex.Message}");
            }
        }

        private bool TryValidateReportInputs(Window win, out DateTime from, out DateTime to)
        {
            from = DateTime.MinValue;
            to = DateTime.MinValue;

            if (SelectedNeighborhood is null)
                return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectNeighborhood"]);

            if (ReportStartDate is null || ReportEndDate is null)
                return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectDates"]);

            if (ReportStartDate > ReportEndDate)
                return ShowError(win, TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_DateOrder"]);

            from = ReportStartDate.Value;
            to = ReportEndDate.Value.AddDays(1).AddSeconds(-1);
            return true;
        }

        private static string? ChooseSavePath(DateTime from, DateTime to, string neighborhoodName)
        {
            var fromStr = from.ToString("dd.MM.yyyy");
            var toStr = to.ToString("dd.MM.yyyy");

            var safeName = string.Concat(neighborhoodName
                .ToLower()
                .Replace(" ", "-")
                .Where(c => char.IsLetterOrDigit(c) || c == '-'));

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = TranslationSource.Instance["Report_SaveTitle"],
                Filter = TranslationSource.Instance["Report_SaveFilter"],
                FileName = TranslationSource.Instance.Format("Report_FileName", neighborhoodName, fromStr, toStr),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        private static void SaveReport(byte[] pdfBytes, string savePath)
        {
            File.WriteAllBytes(savePath, pdfBytes);
        }

        private void AskToOpenReport(Window win, string savePath)
        {
            var confirm = new ConfirmDialog(
                TranslationSource.Instance["Report_Success_Title"],
                TranslationSource.Instance["Report_Success_Message"],
                win);

            if (confirm.ShowDialog() == true)
                OpenPdf(savePath);
        }

        private static void OpenPdf(string filePath)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}