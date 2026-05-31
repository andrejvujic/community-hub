using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class ReportReviewViewModel : INotifyPropertyChanged
    {
        private readonly string _reviewerName;

        public string WindowTitle => $"Prijavi recenziju - {_reviewerName}";

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public ReportReviewViewModel(string reviewerName)
        {
            _reviewerName = reviewerName;

            ConfirmCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty));
            CancelCommand = new RelayCommand<object>(_ => CancelRequested?.Invoke(this, EventArgs.Empty));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
