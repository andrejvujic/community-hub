using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class ReportReviewViewModel : INotifyPropertyChanged
    {
        private readonly string _reviewerName;
        private bool _confirmed;

        public string WindowTitle => $"Prijavi recenziju - {_reviewerName}";

        public bool Confirmed
        {
            get => _confirmed;
            set
            {
                if (_confirmed == value) return;
                _confirmed = value;
                OnPropertyChanged(nameof(Confirmed));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;
        public event EventHandler? CloseRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }

        public ReportReviewViewModel(string reviewerName)
        {
            _reviewerName = reviewerName;

            ConfirmCommand = new RelayCommand<object>(_ => RequestConfirm());
            CancelCommand = new RelayCommand<object>(_ => RequestCancel());
        }

        public void RequestConfirm()
        {
            Confirmed = true;
            ConfirmRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestCancel()
        {
            Confirmed = false;
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
