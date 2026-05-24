using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class RequestNeighborhoodAccessViewModel : INotifyPropertyChanged
    {
        private readonly string _neighborhoodName;
        private bool _confirmed;

        public string NeighborhoodName => _neighborhoodName;

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

        public RequestNeighborhoodAccessViewModel(string neighborhoodName)
        {
            _neighborhoodName = neighborhoodName;

            ConfirmCommand = new RelayCommand<object>(_ => RequestConfirm());
            CancelCommand = new RelayCommand<object>(_ => RequestCancel());
            CloseCommand = new RelayCommand<object>(_ => RequestClose());
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

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
