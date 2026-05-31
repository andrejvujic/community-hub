using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.CoordinatorReviews
{
    public class WriteNewReviewViewModel : INotifyPropertyChanged
    {
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
                    OnPropertyChanged(nameof(RatingText));
                    OnPropertyChanged(nameof(IsCommentRequired));
                    OnPropertyChanged(nameof(CommentLabel));
                    OnPropertyChanged(nameof(IsValid));
                    ((RelayCommand<object>)ConfirmCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string RatingText
        {
            get => _rating > 0 ? _rating.ToString() : "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Rating = 0;
                }
                else if (int.TryParse(value, out int newRating) && newRating <= 5 )
                {
                    Rating = newRating;
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
                    OnPropertyChanged(nameof(IsValid));
                    ((RelayCommand<object>)ConfirmCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsCommentRequired => Rating < 3;

        public string CommentLabel => Rating < 3 ? "Obrazloženje (obavezno):" : "Obrazloženje (opciono):";

        public bool IsValid => Rating >= 1 && Rating <= 5 && (!IsCommentRequired || !string.IsNullOrWhiteSpace(Comment));

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SetRatingCommand { get; }

        public WriteNewReviewViewModel(string neighborhoodName, string coordinatorName)
        {
            NeighborhoodName = neighborhoodName;
            CoordinatorName = coordinatorName;

            ConfirmCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty), _ => IsValid);
            CancelCommand = new RelayCommand<object>(_ => CancelRequested?.Invoke(this, EventArgs.Empty));
            SetRatingCommand = new RelayCommand<object>(param =>
            {
                if (param is int starIndex) Rating = starIndex;
                else if (param is string starStr && int.TryParse(starStr, out int parsed)) Rating = parsed;
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
