using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class FacilityBookingWindowViewModel : INotifyPropertyChanged
    {
        private readonly CityFacilitiesTabViewModel _tabViewModel;
        private readonly Action _closeConfirmed;
        private readonly Action _closeCancelled;
        private AvailableFacilitySlot? _confirmedSlot;

        public ObservableCollection<AvailableFacilitySlot> AvailableSlots { get; } = new();

        public string Title { get; }
        public string Status { get; }
        public string NeighborhoodText { get; }
        public string FacilityText { get; }
        public string? SlotText { get; }
        public bool HasSlotInRange { get; }
        public bool HasNoSlotInRange => !HasSlotInRange;

        private AvailableFacilitySlot? _selectedSlot;
        public AvailableFacilitySlot? SelectedSlot
        {
            get => _selectedSlot;
            set { _selectedSlot = value; OnPropertyChanged(); }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public FacilityBookingWindowViewModel(
            CityFacilityDto facility,
            NeighborhoodDto neighborhood,
            AvailableFacilitySlot? slotInRange,
            List<AvailableFacilitySlot> slotsOutsideRange,
            CityFacilitiesTabViewModel tabViewModel,
            Action closeConfirmed,
            Action closeCancelled)
        {
            _tabViewModel = tabViewModel;
            _closeConfirmed = closeConfirmed;
            _closeCancelled = closeCancelled;
            _confirmedSlot = slotInRange;

            NeighborhoodText = $"Stanovnici kvarta: {neighborhood.Name}";
            FacilityText = $"Objekat: {facility.Name}";
            HasSlotInRange = slotInRange is not null;

            if (slotInRange is not null)
            {
                Title = "Potvrda rezervacije — Termin pronađen";
                Status = $"Sistem je pronašao slobodan termin za posetu objekta {facility.Name} u zadatom opsegu za stanovnike kvarta {neighborhood.Name}.";
                SlotText = $"Termin: {slotInRange.DisplayText}";
            }
            else
            {
                Title = "Potvrda rezervacije — Termin zauzet";
                Status = "Nažalost, nema slobodnih termina u zadatom opsegu. Sistem je pronašao prve slobodne termine van opsega.";
                foreach (var s in slotsOutsideRange)
                    AvailableSlots.Add(s);
                SelectedSlot = AvailableSlots.FirstOrDefault();
            }

            ConfirmCommand = new RelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(_closeCancelled);
        }

        private void ExecuteConfirm()
        {
            var slot = _confirmedSlot ?? SelectedSlot;
            if (slot is null) return;

            _tabViewModel.Book(slot);
            _closeConfirmed();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}