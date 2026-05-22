using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.Views;
using CommunityHub.Ui.Views.BuildingManager;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.BuildingManager
{
    public class HomeWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserMapper _userMapper = new();
        public UserDto UserDto { get; }
        public User User { get; }
        public string Username => UserDto.Name;
        public HomeWindowViewModel(User user)
        {
            UserDto = _userMapper.MapToDto(user);
            User = user;
            SelectedNavItem = "Home";
            CurrentPage = new HomePage(User);
            NavigateCommand = new RelayCommand<string>(NavigateTo);
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

        public ICommand NavigateCommand { get; }
        private void NavigateTo(string? tag)
        {
            if (string.IsNullOrEmpty(tag))
                return;

            SelectedNavItem = tag;

            if (tag == "LogOut")
            {
                HandleLogOut();
                return;
            }

            switch (tag)
            {
                case "Home":
                    CurrentPage = new HomePage(User);
                    break;
                case "Assembly":
                    CurrentPage = new AssemblyPage(User);
                    break;
                case "MyBuildings":
                    CurrentPage = new MyBuildingsPage(User);
                    break;
                case "Registration":
                    CurrentPage = new RegistrationPage(User);
                    break;
                case "Help":
                    break;
            }
        }
        private void HandleLogOut()
        {
            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite da se odjavite?",
                "Pažnja",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
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