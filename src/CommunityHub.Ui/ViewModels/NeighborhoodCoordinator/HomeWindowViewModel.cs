using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views;
using CommunityHub.Ui.Views.NeighborhoodCoordinator;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator
{
    public class HomeWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService = new();
        private long _userId;

        private string _userDisplayName = string.Empty;
        public string UserDisplayName
        {
            get => _userDisplayName;
            private set
            {
                _userDisplayName = value;
                OnPropertyChanged();
            }
        }

        private int _selectedLanguageIndex = 0;
        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                if (_selectedLanguageIndex == value) return;
                _selectedLanguageIndex = value;
                OnPropertyChanged();
                ApplyLanguage();
            }
        }

        private int _selectedThemeIndex = 0;
        public int SelectedThemeIndex
        {
            get => _selectedThemeIndex;
            set
            {
                if (_selectedThemeIndex == value) return;
                _selectedThemeIndex = value;
                OnPropertyChanged();
                ApplyTheme();
            }
        }

        public RelayCommand LogoutCommand { get; }

        public HomeWindowViewModel(long userId)
        {
            _userId = userId;
            LogoutCommand = new RelayCommand(ExecuteLogout);
            LoadUser(userId);
            ((App)System.Windows.Application.Current).ChangeLanguage("sr-RS");

            TranslationSource.Instance.PropertyChanged += (s, e) => LoadUser(_userId);
        }

        public bool LoadUser(long userId)
        {
            var user = _userService.Get(userId);
            if (user is null) return false;

            UserDisplayName = TranslationSource.Instance.Format("Home_UserDisplay", user.Name, user.Surname);
            return true;
        }

        private void ApplyLanguage()
        {
            string culture = SelectedLanguageIndex == 1 ? "en-US" : "sr-RS";
            ((App)System.Windows.Application.Current).ChangeLanguage(culture);
        }

        private void ApplyTheme()
        {
            string themePath = SelectedThemeIndex == 1
                ? "/Resources/Themes/DarkTheme.xaml"
                : "/Resources/Themes/LightTheme.xaml";

            var themeDict = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };
            System.Windows.Application.Current.Resources.MergedDictionaries[0] = themeDict;
        }

        private void ExecuteLogout()
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                         ?? System.Windows.Application.Current.MainWindow;

            var confirmDialog = new ConfirmDialog(
                TranslationSource.Instance["Home_LogoutConfirmTitle"],
                TranslationSource.Instance["Home_LogoutConfirmMessage"],
                window);

            if (confirmDialog.ShowDialog() != true) return;

            LogInForm loginForm = new LogInForm();
            loginForm.Show();

            if (window is HomeWindow homeWindow)
            {
                homeWindow.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}