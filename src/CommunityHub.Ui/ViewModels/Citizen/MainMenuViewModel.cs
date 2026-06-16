using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommunityHub.Ui.ViewModels.Citizen
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private readonly CitizenService _citizenService = new();
        private readonly long _userId;
        private readonly DispatcherTimer _timer;
        private readonly CultureInfo _culture = new("sr-Latn-RS");

        private CitizenDto? _currentUser;
        private string _currentDate = "";
        private string _currentTime = "";

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? WizardHelperRequested;

        public CitizenDto? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value) return;
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                if (_currentDate == value) return;
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                if (_currentTime == value) return;
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public ICommand WizardHelperCommand { get; }
        public ICommand ShowHelpCommand { get; }


        public MainMenuViewModel(long userId)
        {
            _userId = userId;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;

            WizardHelperCommand = new RelayCommand<object>(_ => WizardHelperRequested?.Invoke(this, EventArgs.Empty));
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());

            CurrentUser = _citizenService.GetCitizenById(_userId);
            UpdateDateTime();
            _timer.Start();
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Glavni meni",
                "Pregled vašeg profila i osnovnih informacija. Odavde možete pokrenuti sistem za pomoć.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+W", Description = "Pokreni sistem za pomoć" }
                }
            );
            helpWindow.ShowDialog();
        }

        public void Cleanup()
        {
            _timer.Tick -= Timer_Tick;
            _timer.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e) => UpdateDateTime();

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now;
            CurrentDate = now.ToString("dddd, d.M.yyyy.", _culture);
            CurrentTime = now.ToString("HH:mm", _culture);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
