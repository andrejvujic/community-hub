using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Application.Services.Users;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class MyNeighborhoodCoordinatorViewModel : INotifyPropertyChanged
    {
        private readonly CitizenService _citizenService = new();
        private string _selectedNeighborhoodName = string.Empty;
        private string _coordinatorName = string.Empty;
        private object? _selectedReview;

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
            }
        }

        public ObservableCollection<object> Reviews { get; } = new();

        public ICommand CloseCommand { get; }
        public ICommand WriteNewReviewCommand { get; }
        public ICommand ReportSelectedReviewCommand { get; }

        public MyNeighborhoodCoordinatorViewModel(long userId)
        {
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            WriteNewReviewCommand = new RelayCommand<object>(_ => WriteNewReviewRequested?.Invoke(this, EventArgs.Empty));
            ReportSelectedReviewCommand = new RelayCommand<object>(_ =>
            {
                if (SelectedReview != null) ReportReviewRequested?.Invoke(this, EventArgs.Empty);
            });

            LoadCoordinator(userId);
        }

        private void LoadCoordinator(long userId)
        {
            var citizen = _citizenService.GetCitizenById(userId);

            if (citizen?.Membership?.Neighborhood != null)
            {
                SelectedNeighborhoodName = citizen.Membership.Neighborhood.Name;
                CoordinatorName = $"{citizen.Membership.Neighborhood.Coordinator.Name} {citizen.Membership.Neighborhood.Coordinator.Surname}";
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
