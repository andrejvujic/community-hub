using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.TrustRecords;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class MyNeighborhoodCoordinatorViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CitizenService _citizenService = new();
        private readonly CoordinatorReviewService _reviewService = new();
        private readonly ReviewReportService _reportService = new();
        private readonly CitizenTrustRecordService _trustRecordService = new();
        private string _selectedNeighborhoodName = string.Empty;
        private string _coordinatorName = string.Empty;
        private object? _selectedReview;
        private bool _canReportSelectedReview = true;
        private bool _canWriteNewReview = true;
        private long _neighborhoodId;

        public long UserId => _userId;
        public long NeighborhoodId => _neighborhoodId;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;
        public event EventHandler? WriteNewReviewRequested;
        public event EventHandler? ReportReviewRequested;

        public string SelectedNeighborhoodName
        {
            get => _selectedNeighborhoodName;
            set
            {
                if (_selectedNeighborhoodName == value) return;
                _selectedNeighborhoodName = value;
                OnPropertyChanged(nameof(SelectedNeighborhoodName));
            }
        }

        public string CoordinatorName
        {
            get => _coordinatorName;
            set
            {
                if (_coordinatorName == value) return;
                _coordinatorName = value;
                OnPropertyChanged(nameof(CoordinatorName));
            }
        }

        public object? SelectedReview
        {
            get => _selectedReview;
            set
            {
                if (_selectedReview == value) return;
                _selectedReview = value;
                OnPropertyChanged(nameof(SelectedReview));
                UpdateCanReportSelectedReview();
            }
        }

        public bool CanReportSelectedReview
        {
            get => _canReportSelectedReview;
            private set
            {
                if (_canReportSelectedReview == value) return;
                _canReportSelectedReview = value;
                OnPropertyChanged(nameof(CanReportSelectedReview));
            }
        }

        public string ReportDisabledReason
        {
            get
            {
                if (SelectedReview is not CoordinatorReviewDto review) return "Prvo morate izabrati recenziju";
                if (review.Reviewer.Id == _userId) return "Ne možete prijaviti sopstvenu recenziju";
                return "Već ste prijavili ovu recenziju";
            }
        }

        public string ReportTooltip
        {
            get
            {
                if (!TooltipState.IsEnabled) return null;
                if (!CanReportSelectedReview) return ReportDisabledReason;
                return null;
            }
        }

        public bool CanWriteNewReview
        {
            get => _canWriteNewReview;
            private set
            {
                if (_canWriteNewReview == value) return;
                _canWriteNewReview = value;
                OnPropertyChanged(nameof(CanWriteNewReview));
            }
        }

        public string WriteReviewDisabledReason
        {
            get
            {
                int removedReviewCount = _reviewService.GetRemovedReviewCountByReviewerId(_neighborhoodId, _userId);
                if (removedReviewCount >= 3)  return "Ne možete pisati recenzije jer vam je uklonjeno 3 ili više recenzija";
                return "Već ste napisali recenziju ovog meseca. Možete pisati najviše jednu recenziju mesečno";
            }
        }

        public string WriteReviewTooltip
        {
            get
            {
                if (!TooltipState.IsEnabled) return null;
                if (!CanWriteNewReview) return WriteReviewDisabledReason;
                return null;
            }
        }

        public ObservableCollection<object> Reviews { get; } = new();

        public ICommand CloseCommand { get; }
        public ICommand WriteNewReviewCommand { get; }
        public ICommand ReportSelectedReviewCommand { get; }

        public MyNeighborhoodCoordinatorViewModel(long userId)
        {
            _userId = userId;
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            WriteNewReviewCommand = new RelayCommand<object>(_ => WriteNewReviewRequested?.Invoke(this, EventArgs.Empty), _ => CanWriteNewReview);
            ReportSelectedReviewCommand = new RelayCommand<object>(_ => ReportReviewRequested?.Invoke(this, EventArgs.Empty), _ => SelectedReview != null && CanReportSelectedReview);

            TooltipState.OnPropertyChanged += (s, e) => { OnPropertyChanged(nameof(ReportTooltip)); OnPropertyChanged(nameof(WriteReviewTooltip));};

            LoadData(userId);
        }

        private void UpdateCanReportSelectedReview()
        {
            if (SelectedReview is CoordinatorReviewDto review)
            {
                bool hasUserAlreadyReported = _reportService.HasUserReportedReview(review.Id, _userId);
                CanReportSelectedReview = review.CanUserReport(_userId, hasUserAlreadyReported);
            }
            else
            {
                CanReportSelectedReview = false;
            }

            OnPropertyChanged(nameof(ReportDisabledReason));
            OnPropertyChanged(nameof(ReportTooltip));
            ((RelayCommand<object>)ReportSelectedReviewCommand).RaiseCanExecuteChanged();
        }

        private void LoadData(long userId)
        {
            var citizen = _citizenService.GetCitizenById(userId);
            if (citizen?.Membership?.Neighborhood == null) return;

            _neighborhoodId = citizen.Membership.Neighborhood.Id;
            SelectedNeighborhoodName = citizen.Membership.Neighborhood.Name;
            CoordinatorName = $"{citizen.Membership.Neighborhood.Coordinator.Name} {citizen.Membership.Neighborhood.Coordinator.Surname}";

            LoadReviews();
            UpdateCanWriteNewReview();
        }

        private void LoadReviews()
        {
            var selectedReviewId = (SelectedReview as CoordinatorReviewDto)?.Id;
            var reviews = _reviewService.GetByNeighborhoodId(_neighborhoodId);
            Reviews.Clear();
            foreach (var review in reviews) Reviews.Add(review);

            SelectedReview = Reviews.FirstOrDefault(r => r is CoordinatorReviewDto dto && dto.Id == selectedReviewId) ?? Reviews.FirstOrDefault();
        }

        public void ExecuteReport(long reviewId)
        {
            var review = _reviewService.GetById(reviewId);
            var reporter = _citizenService.GetCitizenById(_userId);
            if (review == null || reporter == null) return;

            var reportDto = new ReviewReportDto(0, review, reporter, DateTime.Now);
            _reportService.Create(reportDto);

            int totalReports = _reportService.GetReportCountByReviewId(reviewId);
            int trustedReports = CountTrustedReports(reviewId);

            if (review.ShouldBeRemoved(totalReports, trustedReports))
            {
                _reviewService.MarkAsRemoved(reviewId);
                UpdateCanWriteNewReview();
            }

            LoadReviews();
        }

        public void CreateReview(int rating, string? comment)
        {
            var reviewer = _citizenService.GetCitizenById(_userId);
            var neighborhood = reviewer!.Membership?.Neighborhood;
            if (reviewer == null || neighborhood == null) return;

            var reviewDto = new CoordinatorReviewDto(0, rating, string.IsNullOrWhiteSpace(comment) ? null : comment, DateTime.Now, neighborhood, reviewer, false);

            _reviewService.Create(reviewDto);
            LoadReviews();
            UpdateCanWriteNewReview();
        }

        private int CountTrustedReports(long reviewId)
        {
            var reports = _reportService.GetByReviewId(reviewId);
            int trustedCount = 0;

            foreach (var report in reports)
            {
                var trustRecord = _trustRecordService.GetTrustRecord(report.Reporter.Id);
                if (report.IsTrusted(trustRecord!.TrustStatus))
                {
                    trustedCount++;
                }
            }

            return trustedCount;
        }

        private void UpdateCanWriteNewReview()
        {
            int removedReviewCount = _reviewService.GetRemovedReviewCountByReviewerId(_neighborhoodId, _userId);
            var allReviews = _reviewService.GetByNeighborhoodId(_neighborhoodId);
            var userRecentReview = allReviews.FirstOrDefault(r => r.Reviewer.Id == _userId && r.CreatedAt.Year == DateTime.Now.Year && r.CreatedAt.Month == DateTime.Now.Month);

            var reviewer = _citizenService.GetCitizenById(_userId);
            CanWriteNewReview = reviewer!.CanWriteCoordinatorReview(removedReviewCount, userRecentReview != null);

            OnPropertyChanged(nameof(WriteReviewDisabledReason));
            OnPropertyChanged(nameof(WriteReviewTooltip));
            ((RelayCommand<object>)WriteNewReviewCommand).RaiseCanExecuteChanged();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
