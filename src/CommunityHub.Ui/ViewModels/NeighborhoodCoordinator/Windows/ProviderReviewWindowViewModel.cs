using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.JobListingBookings;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ProviderReviewWindowViewModel : INotifyPropertyChanged
    {
        private readonly ProviderReviewService _providerReviewService = new();
        private readonly BookedJobWithReviewDto _bookedJobWithReview;
        private readonly UserDto _coordinator;

        public string Title => _bookedJobWithReview.Review == null
            ? $"Ocenite uslugu: {_bookedJobWithReview.ServiceName}"
            : $"Izmenite recenziju: {_bookedJobWithReview.ServiceName}";

        public bool IsEditing => _bookedJobWithReview.Review != null;
        public int QualityIndex => IsEditing ? ExistingQuality - 1 : -1;
        public int CommunicationIndex => IsEditing ? ExistingCommunication - 1 : -1;

        private int ExistingQuality => _bookedJobWithReview.Review?.QualityRating ?? 0;
        private int ExistingCommunication => _bookedJobWithReview.Review?.CommunicationRating ?? 0;

        private string _comment;
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

        private int _selectedQualityIndex;
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

        private int _selectedCommunicationIndex;
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

        public ProviderReviewWindowViewModel(BookedJobWithReviewDto bookedJobWithReview, UserDto coordinator)
        {
            _bookedJobWithReview = bookedJobWithReview;
            _coordinator = coordinator;
            _comment = _bookedJobWithReview.Review?.Comment ?? string.Empty;
            _selectedQualityIndex = QualityIndex;
            _selectedCommunicationIndex = CommunicationIndex;
        }

        public (bool Success, string? Error) TrySubmit()
        {
            if (SelectedQualityIndex < 0 || SelectedCommunicationIndex < 0)
                return (false, "Molimo odaberite obe ocene.");

            if (string.IsNullOrWhiteSpace(Comment))
                return (false, "Komentar je obavezan deo recenzije.");

            int quality = SelectedQualityIndex + 1;
            int communication = SelectedCommunicationIndex + 1;

            SavedReview = IsEditing
                ? _providerReviewService.Update(_bookedJobWithReview.Review!, quality, communication, Comment.Trim())
                : _providerReviewService.Create(_bookedJobWithReview.BookedJob, _coordinator, quality, communication, Comment.Trim());

            return (true, null);
        }

        public ProviderReviewDto? SavedReview { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}