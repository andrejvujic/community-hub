using System.Windows;
using CommunityHub.Application.Domain.Services;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class BookingConfirmationWindow : Window
    {
        public bool Confirmed { get; private set; }

        public BookingConfirmationWindow(JobListing listing, AvailableJobSlot slot)
        {
            InitializeComponent();
            Populate(listing, slot);
        }

        private void Populate(JobListing listing, AvailableJobSlot slot)
        {
            PopulateHeader(listing);
            PopulateSlotInfo(listing.BillingType, slot);
            PopulatePriceChange(listing, slot);
        }

        private void PopulateHeader(JobListing listing)
        {
            HeaderTextBlock.Text =
                $"Sistem je pronašao prvi slobodan termin za uslugu {listing.ServiceName} " +
                $"(Serviser: {listing.Provider.Name} {listing.Provider.Surname}).";
        }

        private void PopulateSlotInfo(BillingType billingType, AvailableJobSlot slot)
        {
            if (billingType == BillingType.PerHour)
                PopulatePerHourSlotInfo(slot);
            else
                PopulatePerJobSlotInfo(slot);
        }

        private void PopulatePerHourSlotInfo(AvailableJobSlot slot)
        {
            SlotDateTextBlock.Text = $"Pronađeni termin: {slot.Date:dd.MM.yyyy.} u {slot.Time!.Value:HH:mm}";

            DurationTextBlock.Visibility = Visibility.Visible;
            DurationTextBlock.Text = $"Trajanje: {slot.DurationHours} sata";

            HourlyRateTextBlock.Visibility = Visibility.Visible;
            HourlyRateTextBlock.Text = $"Cena po satu: {slot.Price:N2} RSD";

            TotalAmountTextBlock.Visibility = Visibility.Visible;
            TotalAmountTextBlock.Text = $"Ukupan iznos za uplatu: {(slot.Price * slot.DurationHours):N2} RSD";
        }

        private void PopulatePerJobSlotInfo(AvailableJobSlot slot)
        {
            SlotDateTextBlock.Text = $"Pronađeni termin: {slot.Date:dd.MM.yyyy.}";

            CenaTextBlock.Visibility = Visibility.Visible;
            CenaTextBlock.Text = $"Cena: {slot.Price:N2} RSD";
        }

        private void PopulatePriceChange(JobListing listing, AvailableJobSlot slot)
        {
            if (!slot.IsPriceChanged) return;

            PriceChangePanel.Visibility = Visibility.Visible;
            PriceChangeInfoTextBlock.Text = BuildPriceChangeText(listing, slot);
            QuestionTextBlock.Text = "Da li želite da zakažete ovaj termin po ovoj ceni?";
        }

        private static string BuildPriceChangeText(JobListing listing, AvailableJobSlot slot)
        {
            var newEntry = listing.PriceList
                .OrderBy(p => p.ValidFrom)
                .FirstOrDefault(p => p.IsActiveOn(slot.Date));

            string validFromStr = newEntry is not null
                ? newEntry.ValidFrom.ToString("dd.MM.yyyy.")
                : slot.Date.ToString("dd.MM.yyyy.");

            return $"Prvi slobodan termin nije u opsegu datuma za koji važi trenutna cena.\n" +
                   $"Nova cena koja važi od {validFromStr} iznosi {slot.Price:N2} RSD.";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
            Close();
        }
    }
}
