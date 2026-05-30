using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.TrustRecords;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.TrustRecords;
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
        private long _neighborhoodId;

        public long UserId => _userId;

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

        public ObservableCollection<object> Reviews { get; } = new();

        public ICommand CloseCommand { get; }
        public ICommand WriteNewReviewCommand { get; }
        public ICommand ReportSelectedReviewCommand { get; }

        public MyNeighborhoodCoordinatorViewModel(long userId)
        {
            _userId = userId;
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            WriteNewReviewCommand = new RelayCommand<object>(_ => WriteNewReviewRequested?.Invoke(this, EventArgs.Empty));
            ReportSelectedReviewCommand = new RelayCommand<object>(_ => ReportReviewRequested?.Invoke(this, EventArgs.Empty), _ => SelectedReview != null && CanReportSelectedReview);

            LoadData(userId);
        }

        private void UpdateCanReportSelectedReview()
        {
            if (SelectedReview is CoordinatorReviewDto review)
            {
                CanReportSelectedReview = !_reportService.HasUserReportedReview(review.Id, _userId);
            }
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
        }

        private void LoadReviews()
        {
            var reviews = _reviewService.GetByNeighborhoodId(_neighborhoodId);
            Reviews.Clear();
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }
            if (Reviews.Count > 0)
            {
                SelectedReview = Reviews[0];
            }
        }

        public void ExecuteReport(long reviewId)
        {
            var review = _reviewService.GetById(reviewId);
            var reporter = _citizenService.GetCitizenById(_userId);
            if (review == null || reporter == null) return;

            var reportDto = new ReviewReportDto(0, review, reporter, DateTime.Now);
            _reportService.Create(reportDto);

            int totalReports = _reportService.GetReportCountByReviewId(reviewId);
            if (totalReports >= 10 || CountTrustedReports(reviewId) >= 5)
            {
                _reviewService.MarkAsRemoved(reviewId);
            }

            LoadReviews();
        }

        private int CountTrustedReports(long reviewId)
        {
            var reports = _reportService.GetByReviewId(reviewId);
            int trustedCount = 0;

            foreach (var report in reports)
            {
                var trustRecord = _trustRecordService.GetTrustRecord(report.Reporter.Id);
                if (trustRecord!.TrustStatus == TrustStatus.Prominent || trustRecord.TrustStatus == TrustStatus.Trustworthy)
                {
                    trustedCount++;
                }
            }

            return trustedCount;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
