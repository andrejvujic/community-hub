using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ServiceBookingConfirmationWindowViewModel : INotifyPropertyChanged
    {
        private readonly JobListingDto _jobListing;
        private readonly AvailableJobSlotDto _availableJobSlot;

        public string Header =>
            TranslationSource.Instance.Format("ServiceBooking_Header",
                _jobListing.ServiceName,
                _jobListing.Provider.Name,
                _jobListing.Provider.Surname);
        public string SlotDateText => _availableJobSlot.Time.HasValue
            ? TranslationSource.Instance.Format("ServiceBooking_SlotDateTime",
                TranslationSource.Instance.FormatDate(_availableJobSlot.Date),
                TranslationSource.Instance.FormatTime(_availableJobSlot.Time.Value))
            : TranslationSource.Instance.Format("ServiceBooking_SlotDate",
                TranslationSource.Instance.FormatDate(_availableJobSlot.Date));

        public string DurationText =>
            TranslationSource.Instance.Format("ServiceBooking_Duration", _availableJobSlot.DurationHours);

        public string HourlyRateText =>
            TranslationSource.Instance.Format("ServiceBooking_HourlyRate", _availableJobSlot.Price);

        public string TotalAmountText =>
            TranslationSource.Instance.Format("ServiceBooking_Total",
                _availableJobSlot.Price * _availableJobSlot.DurationHours);

        public string PerJobPriceText =>
            TranslationSource.Instance.Format("ServiceBooking_PerJobPrice", _availableJobSlot.Price);

        public string QuestionText => IsPriceChanged
            ? TranslationSource.Instance["ServiceBooking_QuestionPriceChanged"]
            : TranslationSource.Instance["ServiceBooking_Question"];
        public bool IsPerHour => _jobListing.BillingType == BillingType.PerHour;
        public bool IsPerJob => !IsPerHour;
        public bool IsPriceChanged => _availableJobSlot.IsPriceChanged;

        public Visibility PerHourVisibility => IsPerHour ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PerJobVisibility => IsPerJob ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PriceChangeVisibility => IsPriceChanged ? Visibility.Visible : Visibility.Collapsed;

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ServiceBookingConfirmationWindowViewModel(JobListingDto jobListing, AvailableJobSlotDto availableJobSlot)
        {
            _jobListing = jobListing;
            _availableJobSlot = availableJobSlot;

            ConfirmCommand = new RelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private string BuildPriceChangeInfo()
        {
            var newEntry = _jobListing.PriceList
                .OrderBy(p => p.ValidFrom)
                .FirstOrDefault(p => p.IsActiveOn(_availableJobSlot.Date));

            string validFrom = TranslationSource.Instance.FormatDate(newEntry?.ValidFrom ?? _availableJobSlot.Date);

            return TranslationSource.Instance.Format("ServiceBooking_PriceChangeInfo", validFrom, _availableJobSlot.Price);
        }

        private void ExecuteConfirm() => CloseWindow(true);

        private void ExecuteCancel() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (window is ServiceBookingConfirmationWindow view)
            {
                view.DialogResult = result;
                view.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}