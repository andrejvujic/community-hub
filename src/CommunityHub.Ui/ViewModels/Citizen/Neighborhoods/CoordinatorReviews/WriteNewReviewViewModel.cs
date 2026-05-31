using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class WriteNewReviewViewModel : INotifyPropertyChanged
    {
        private readonly long _coordinatorId;
        private int _rating = 5;
        private string _comment = "";

        public string NeighborhoodName { get; }
        public string CoordinatorName { get; }
        public string ReviewDate => DateTime.Now.ToString("d.M.yyyy");
        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                    OnPropertyChanged(nameof(IsCommentRequired));
                    OnPropertyChanged(nameof(CommentLabel));
                }
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public bool IsCommentRequired => Rating < 3;

        public string CommentLabel => Rating < 3 ? "Obrazloženje (obavezno):" : "Obrazloženje (opciono):";

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public WriteNewReviewViewModel(long coordinatorId, string neighborhoodName, string coordinatorName)
        {
            _coordinatorId = coordinatorId;
            NeighborhoodName = neighborhoodName;
            CoordinatorName = coordinatorName;

            ConfirmCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty));
            CancelCommand = new RelayCommand<object>(_ => CancelRequested?.Invoke(this, EventArgs.Empty));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
