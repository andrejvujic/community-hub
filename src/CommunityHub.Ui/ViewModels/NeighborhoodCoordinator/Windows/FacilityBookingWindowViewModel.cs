using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class FacilityBookingWindowViewModel : INotifyPropertyChanged
    {
        private readonly CityFacilitiesTabViewModel _tabViewModel;
        private AvailableFacilitySlotDto? _confirmedSlot;

        public ObservableCollection<AvailableFacilitySlotDto> AvailableSlots { get; } = new();

        public string Title { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public string NeighborhoodText { get; }
        public string FacilityText { get; }
        public string? SlotText { get; private set; }
        public bool HasSlotInRange { get; }
        public bool HasNoSlotInRange => !HasSlotInRange;

        private AvailableFacilitySlotDto? _selectedSlot;
        public AvailableFacilitySlotDto? SelectedSlot
        {
            get => _selectedSlot;
            set { _selectedSlot = value; OnPropertyChanged(); }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public FacilityBookingWindowViewModel(
            CityFacilityDto facility, NeighborhoodDto neighborhood,
            AvailableFacilitySlotDto? slotInRange, List<AvailableFacilitySlotDto> slotsOutsideRange,
            CityFacilitiesTabViewModel tabViewModel)
        {
            _tabViewModel = tabViewModel;
            _confirmedSlot = slotInRange;
            NeighborhoodText = TranslationSource.Instance.Format("FacilityBooking_Neighborhood", neighborhood.Name);
            FacilityText = TranslationSource.Instance.Format("FacilityBooking_Facility", facility.Name);
            HasSlotInRange = slotInRange is not null;

            SetupStatusText(facility, neighborhood, slotInRange);
            SetupOutsideSlots(slotsOutsideRange);

            ConfirmCommand = new RelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void SetupStatusText(CityFacilityDto facility, NeighborhoodDto neighborhood, AvailableFacilitySlotDto? slot)
        {
            if (slot is not null)
            {
                Title = TranslationSource.Instance["FacilityBooking_TitleFound"];
                Status = TranslationSource.Instance.Format("FacilityBooking_StatusFound", facility.Name, neighborhood.Name);
                SlotText = TranslationSource.Instance.Format("FacilityBooking_SlotText", FormatSlot(slot));
            }
            else
            {
                Title = TranslationSource.Instance["FacilityBooking_TitleNotFound"];
                Status = TranslationSource.Instance["FacilityBooking_StatusNotFound"];
            }
        }

        private static string FormatSlot(AvailableFacilitySlotDto slot)
        {
            var date = TranslationSource.Instance.FormatDate(slot.Start);
            var time = TranslationSource.Instance.FormatTime(TimeOnly.FromDateTime(slot.Start));
            var at = TranslationSource.Instance["Service_At"];
            return $"{date} {at} {time} ({slot.DurationHours}h)";
        }

        private void SetupOutsideSlots(List<AvailableFacilitySlotDto> slotsOutsideRange)
        {
            if (HasSlotInRange) return;

            foreach (var s in slotsOutsideRange)
                AvailableSlots.Add(s);

            SelectedSlot = AvailableSlots.FirstOrDefault();
        }

        private void ExecuteConfirm()
        {
            var slot = _confirmedSlot ?? SelectedSlot;
            if (slot is null) return;

            _tabViewModel.Book(slot);
            CloseWindow(true);
        }

        private void ExecuteCancel() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = System.Windows.Application.Current.Windows
                .OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (window is FacilityBookingWindow dialog)
            {
                dialog.DialogResult = result;
                dialog.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}