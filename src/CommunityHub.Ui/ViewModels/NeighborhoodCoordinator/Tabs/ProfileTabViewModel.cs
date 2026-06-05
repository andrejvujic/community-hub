using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class ProfileTabViewModel : INotifyPropertyChanged
    {
        private readonly CoordinatorReviewService _reviewService = new();
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly UserService _userService = new();
        private readonly NeighborhoodAccessRequestService _requestService = new();
        private readonly NeighborhoodMembershipService _membershipService = new();
        private long _coordinatorId;

        public ObservableCollection<CoordinatorReviewDto> Reviews { get; } = new();

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public int CitizenCount { get; private set; }
        public int NeighborhoodCount { get; private set; }
        public int AcceptedRequestCount { get; private set; }
        public int RejectedRequestCount { get; private set; }
        public int PendingRequestCount { get; private set; }

        private double _averageRating;
        public double AverageRating
        {
            get => _averageRating;
            private set { _averageRating = value; OnPropertyChanged(); }
        }

        private double _averageRatingLastYear;
        public double AverageRatingLastYear
        {
            get => _averageRatingLastYear;
            private set { _averageRatingLastYear = value; OnPropertyChanged(); }
        }

        public ProfileTabViewModel()
        {
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadData();
        }

        public void LoadData()
        {
            LoadUserInfo();
            LoadReviews();
            LoadStats();
        }

        private void LoadUserInfo()
        {
            var user = _userService.Get(_coordinatorId);
            if (user is null) return;
            FirstName = user.Name;
            LastName = user.Surname;
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
        }

        private void LoadReviews()
        {
            Reviews.Clear();
            var reviews = _reviewService.GetByCoordinatorId(_coordinatorId);

            foreach (var r in reviews)
                Reviews.Add(r);

            AverageRating = reviews.Count > 0
                ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;

            var oneYearAgo = DateTime.Now.AddYears(-1);
            var lastYear = reviews.Where(r => r.CreatedAt >= oneYearAgo).ToList();
            AverageRatingLastYear = lastYear.Count > 0
                ? Math.Round(lastYear.Average(r => r.Rating), 1) : 0;
        }

        private void LoadStats()
        {
            NeighborhoodCount = _neighborhoodService.GetNeighborhoodCountsByCoordinator(_coordinatorId);
            CitizenCount = _membershipService.GetCitizenCountByCoordinator(_coordinatorId);
            OnPropertyChanged(nameof(CitizenCount));
            OnPropertyChanged(nameof(NeighborhoodCount));

            var (accepted, rejected, pending) = _requestService.GetRequestStatsByCoordinator(_coordinatorId);
            AcceptedRequestCount = accepted;
            RejectedRequestCount = rejected;
            PendingRequestCount = pending;
            OnPropertyChanged(nameof(AcceptedRequestCount));
            OnPropertyChanged(nameof(RejectedRequestCount));
            OnPropertyChanged(nameof(PendingRequestCount));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}