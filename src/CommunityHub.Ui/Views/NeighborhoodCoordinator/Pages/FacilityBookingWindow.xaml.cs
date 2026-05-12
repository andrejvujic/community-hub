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
using System.Windows.Shapes;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.CityFacility;
using CommunityHub.Application.Services;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for FacilityBookingWindow.xaml
    /// </summary>
    public partial class FacilityBookingWindow : Window
    {
        private readonly CityFacility _facility;
        private readonly Neighborhood _neighborhood;
        private readonly FacilityBookingService _bookingService;
        private AvailableFacilitySlot? _confirmedSlot;

        public FacilityReservation? CreatedReservation { get; private set; }

        public ObservableCollection<AvailableFacilitySlot> AvailableSlots { get; set; } = new ObservableCollection<AvailableFacilitySlot>();

        public FacilityBookingWindow(
            CityFacility facility,
            Neighborhood neighborhood,
            AvailableFacilitySlot? slotInRange,
            List<AvailableFacilitySlot> slotsOutsideRange,
            FacilityBookingService bookingService)
        {
            InitializeComponent();
            DataContext = this;

            _facility = facility;
            _neighborhood = neighborhood;
            _bookingService = bookingService;

            Populate(slotInRange, slotsOutsideRange);
        }

        private void Populate(
            AvailableFacilitySlot? slotInRange,
            List<AvailableFacilitySlot> slotsOutsideRange)
        {
            PopulateDetails();

            if (slotInRange is not null)
                PopulateFoundInRange(slotInRange);
            else
                PopulateFoundOutsideRange(slotsOutsideRange);
        }

        private void PopulateDetails()
        {
            NeighborhoodTextBlock.Text = $"Stanovnici kvarta: {_neighborhood.Name}";
            FacilityTextBlock.Text = $"Objekat: {_facility.Name}";
        }

        private void PopulateFoundInRange(AvailableFacilitySlot slot)
        {
            TitleTextBlock.Text = "Potvrda rezervacije — Termin pronađen";
            StatusTextBlock.Text =
                $"Sistem je pronašao slobodan termin za posetu objekta {_facility.Name} " +
                $"u zadatom opsegu za stanovnike kvarta {_neighborhood.Name}.";

            SlotTextBlock.Text = $"Termin: {slot.DisplayText}";
            _confirmedSlot = slot;
        }

        private void PopulateFoundOutsideRange(List<AvailableFacilitySlot> slots)
        {
            TitleTextBlock.Text = "Potvrda rezervacije — Termin zauzet";
            StatusTextBlock.Text =
                $"Nažalost, nema slobodnih termina u zadatom opsegu. " +
                $"Sistem je pronašao prve slobodne termine van opsega.";

            SlotTextBlock.Visibility = Visibility.Collapsed;
            ComboPanel.Visibility = Visibility.Visible;

            foreach (var slot in slots)
                AvailableSlots.Add(slot);

            SlotsComboBox.ItemsSource = AvailableSlots;
            if (AvailableSlots.Count > 0)
                SlotsComboBox.SelectedIndex = 0;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var slot = ResolveSelectedSlot();
            if (slot is null) return;

            CreatedReservation = _bookingService.BookSlot(_facility, _neighborhood, slot);
            DialogResult = true;
            Close();
        }

        private AvailableFacilitySlot? ResolveSelectedSlot()
        {
            if (_confirmedSlot is not null) return _confirmedSlot;

            if (SlotsComboBox.SelectedItem is not AvailableFacilitySlot selected)
            {
                MessageBox.Show("Odaberite termin.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            return selected;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
