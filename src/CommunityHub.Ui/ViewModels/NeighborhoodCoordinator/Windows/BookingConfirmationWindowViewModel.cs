using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class BookingConfirmationWindowViewModel : INotifyPropertyChanged
    {
        private readonly JobListingDto _jobListing;
        private readonly AvailableJobSlotDto _availableJobSlot;

        public string Header =>
            $"Sistem je pronašao prvi slobodan termin za uslugu: {_jobListing.ServiceName} " +
            $"(Serviser: {_jobListing.Provider.Name} {_jobListing.Provider.Surname}).";
        public string SlotDateText => _availableJobSlot.Time.HasValue
            ? $"Pronađeni termin: {_availableJobSlot.Date:dd.MM.yyyy.} u {_availableJobSlot.Time.Value:HH:mm}"
            : $"Pronađeni termin: {_availableJobSlot.Date:dd.MM.yyyy.}";

        public string DurationText => $"Trajanje: {_availableJobSlot.DurationHours} sata";
        public string HourlyRateText => $"Cena po satu: {_availableJobSlot.Price:N2} RSD";
        public string TotalAmountText => $"Ukupan iznos za uplatu: {_availableJobSlot.Price * _availableJobSlot.DurationHours:N2} RSD";
        public string PerJobPriceText => $"Cena: {_availableJobSlot.Price:N2} RSD";
        public string PriceChangeInfo => BuildPriceChangeInfo();

        public string QuestionText => IsPriceChanged
            ? "Da li želite da zakažete ovaj termin po ovoj ceni?"
            : "Da li želite da zakažete ovaj termin?";

        public bool IsPerHour => _jobListing.BillingType == BillingType.PerHour;
        public bool IsPerJob => !IsPerHour;
        public bool IsPriceChanged => _availableJobSlot.IsPriceChanged;

        public Visibility PerHourVisibility => IsPerHour ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PerJobVisibility => IsPerJob ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PriceChangeVisibility => IsPriceChanged ? Visibility.Visible : Visibility.Collapsed;

        public BookingConfirmationWindowViewModel(JobListingDto jobListing, AvailableJobSlotDto availableJobSlot)
        {
            _jobListing = jobListing;
            _availableJobSlot = availableJobSlot;
        }

        private string BuildPriceChangeInfo()
        {
            var newEntry = _jobListing.PriceList
                .OrderBy(p => p.ValidFrom)
                .FirstOrDefault(p => p.IsActiveOn(_availableJobSlot.Date));

            string validFrom = newEntry?.ValidFrom.ToString("dd.MM.yyyy.")
                               ?? _availableJobSlot.Date.ToString("dd.MM.yyyy.");

            return $"Prvi slobodan termin nije u opsegu datuma za koji važi trenutna cena.\n" +
                   $"Nova cena koja važi od {validFrom} iznosi {_availableJobSlot.Price:N2} RSD.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}