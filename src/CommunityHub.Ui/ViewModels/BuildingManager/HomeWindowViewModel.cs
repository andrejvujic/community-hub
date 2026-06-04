using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.Views;
using CommunityHub.Ui.Views.BuildingManager;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Ui.Helpers.BuildingManagerAlerts;

namespace CommunityHub.Ui.ViewModels.BuildingManager
{
    public class HomeWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserMapper _userMapper = new();
        private readonly IAppNavigationService _navigationService;

        public UserDto UserDto { get; }
        public User User { get; }

        public string Username => UserDto.Name;

        public HomeWindowViewModel(User user, IAppNavigationService navigationService)
        {
            _navigationService = navigationService;

            UserDto = _userMapper.MapToDto(user);
            User = user;

            NavigateCommand = new RelayCommand<string>(NavigateTo);
            BackCommand = new RelayCommand<object>(GoBack, _ => CanGoBack);
            ForwardCommand = new RelayCommand<object>(GoForward, _ => CanGoForward);

            _navigationService.Navigated += OnNavigated;

            _navigationService.Navigate(new AppNavigationState
            {
                PageTag = "Home"
            });
        }

        private string? _selectedNavItem;
        public string? SelectedNavItem
        {
            get => _selectedNavItem;
            set
            {
                _selectedNavItem = value;
                OnPropertyChanged();
            }
        }

        private Page? _currentPage;
        public Page? CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public bool CanGoBack => _navigationService.CanGoBack;
        public bool CanGoForward => _navigationService.CanGoForward;

        public ICommand NavigateCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ForwardCommand { get; }

        private void NavigateTo(string? tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return;

            if (tag == "LogOut")
            {
                HandleLogOut();
                return;
            }

            _navigationService.Navigate(new AppNavigationState
            {
                PageTag = tag
            });

            RefreshNavigationButtons();
        }

        private void GoBack(object? parameter)
        {
            _navigationService.GoBack();
            RefreshNavigationButtons();
        }

        private void GoForward(object? parameter)
        {
            _navigationService.GoForward();
            RefreshNavigationButtons();
        }

        private void OnNavigated(AppNavigationState state)
        {
            switch (state.PageTag)
            {
                case "Home":
                    SelectedNavItem = "Home";
                    CurrentPage = new HomePage(User);
                    break;

                case "Assembly":
                    SelectedNavItem = "Assembly";
                    CurrentPage = new AssemblyPage(User);
                    break;

                case "MyBuildings":
                    SelectedNavItem = "MyBuildings";
                    CurrentPage = new MyBuildingsPage(User, _navigationService);
                    break;

                case "Registration":
                    SelectedNavItem = "Registration";
                    CurrentPage = new RegistrationPage(User);
                    break;

                case "BuildingDetails":
                    SelectedNavItem = "MyBuildings";
                    if (state.Building != null)
                    {
                        CurrentPage = new BuildingDetailsPage(User, state.Building, _navigationService, state.SectionTag);
                    }
                    break;

                case "Help":
                    SelectedNavItem = "Help";
                    break;
            }

            RefreshNavigationButtons();
        }

        private void RefreshNavigationButtons()
        {
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
        }

        private void HandleLogOut()
        {
            var dialogService = new DialogService();

            bool confirm = dialogService.ShowConfirmation("Pažnja",
                "Da li ste sigurni da želite da se odjavite?");

            if (confirm)
            {
                LogInForm logInForm = new LogInForm();
                logInForm.Show();

                HomeWindow? homeWindow = System.Windows.Application.Current.Windows
                    .OfType<HomeWindow>()
                    .FirstOrDefault();

                homeWindow?.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}