using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.JobListingBookings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ProviderReviewWindowViewModel : INotifyPropertyChanged
    {
        private readonly ProviderReviewService _providerReviewService = new();
        private readonly BookedJobDto _bookedJob;
        private readonly UserDto _coordinator;

        public string Title => _bookedJob.Review == null
            ? $"Ocenite uslugu: {_bookedJob.Listing.ServiceName}"
            : $"Izmenite recenziju: {_bookedJob.Listing.ServiceName}";

        public bool IsEditing => _bookedJob.Review != null;

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set 
            { 
                _comment = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(CanConfirm)); 
            }
        }

        private int _selectedQualityIndex = -1;
        public int SelectedQualityIndex
        {
            get => _selectedQualityIndex;
            set 
            {
                _selectedQualityIndex = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(CanConfirm)); 
            }
        }

        private int _selectedCommunicationIndex = -1;
        public int SelectedCommunicationIndex
        {
            get => _selectedCommunicationIndex;
            set 
            { 
                _selectedCommunicationIndex = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(CanConfirm)); 
            }
        }

        public bool CanConfirm =>
            SelectedQualityIndex >= 0 &&
            SelectedCommunicationIndex >= 0 &&
            !string.IsNullOrWhiteSpace(Comment);

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ProviderReviewDto? SavedReview { get; private set; }
        public bool? DialogResult { get; private set; }

        public ProviderReviewWindowViewModel(BookedJobDto bookedJob, UserDto coordinator)
        {
            _bookedJob = bookedJob;
            _coordinator = coordinator;

            if (IsEditing)
            {
                Comment = _bookedJob.Review!.Comment ?? string.Empty;
                SelectedQualityIndex = _bookedJob.Review.QualityRating - 1;
                SelectedCommunicationIndex = _bookedJob.Review.CommunicationRating - 1;
            }

            ConfirmCommand = new RelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteConfirm()
        {
            if (!ValidateInput()) return;

            int quality = SelectedQualityIndex + 1;
            int communication = SelectedCommunicationIndex + 1;

            if (IsEditing)
            {
                SavedReview = _providerReviewService.Update(_bookedJob.Review!, quality, communication, Comment.Trim());
            }
            else
            {
                SavedReview = _providerReviewService.Create(_bookedJob, _coordinator, quality, communication, Comment.Trim());
            }

            CloseWindow(true);
        }

        private bool ValidateInput()
        {
            var activeWindow = GetActiveWindow();

            if (SelectedQualityIndex < 0)
            {
                new InfoDialog("Nepotpuni podaci", "Molimo Vas da izaberete ocenu za kvalitet izvršene usluge.", activeWindow).ShowDialog();
                return false;
            }

            if (SelectedCommunicationIndex < 0)
            {
                new InfoDialog("Nepotpuni podaci", "Molimo Vas da izaberete ocenu za komunikaciju sa serviserom.", activeWindow).ShowDialog();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Comment))
            {
                new InfoDialog("Nepotpuni podaci", "Molimo Vas da unesete komentar. Obrazloženje recenzije je obavezno.", activeWindow).ShowDialog();
                return false;
            }

            return true;
        }

        private void ExecuteCancel() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = GetActiveWindow();
            if (window is ProviderReviewWindow view)
            {
                view.DialogResult = result;
                view.Close();
            }
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}