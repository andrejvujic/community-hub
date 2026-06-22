using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CommunityHub.Ui.ViewModels.Citizen.Help
{
    public class HelpWizardViewModel : INotifyPropertyChanged
    {
        private List<WizardStep> _steps = new();
        private int _currentIndex = 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public WizardStep? CurrentStep => _steps.Count > 0 && _currentIndex < _steps.Count
            ? _steps[_currentIndex]
            : null;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex == value) return;
                _currentIndex = value;
                OnPropertyChanged(nameof(CurrentIndex));
                OnPropertyChanged(nameof(CurrentStep));
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(CanGoForward));
                OnPropertyChanged(nameof(IsLastStep));
                OnPropertyChanged(nameof(ProgressText));
            }
        }

        public bool CanGoBack => _currentIndex > 0;
        public bool CanGoForward => _currentIndex < _steps.Count - 1;
        public bool IsLastStep => _currentIndex == _steps.Count - 1;
        public string ProgressText => $"{_currentIndex + 1} / {_steps.Count}";

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand CloseCommand { get; }

        public HelpWizardViewModel()
        {
            NextCommand = new RelayCommand<object>(_ => { if (CanGoForward) CurrentIndex++; });
            BackCommand = new RelayCommand<object>(_ => { if (CanGoBack) CurrentIndex--; });
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
        }

        public void SetSteps(List<WizardStep> steps)
        {
            _steps = steps;
            CurrentIndex = 0;
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class WizardStep
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public BitmapImage? Image { get; set; }
}