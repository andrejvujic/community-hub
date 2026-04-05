using System.Windows;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Views;

namespace CommunityHub.Ui.Views;

public partial class LogInForm : Window
{
    private readonly UserDbRepository _userRepository;

    public LogInForm()
    {
        InitializeComponent();
        _userRepository = new UserDbRepository();
        
        // Uncomment for auto login:
        // HandleLogin("setfan.matic", "123");
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text;
        string password = PasswordBox.Password;

        HandleLogin(username, password);
    }

    private void HandleLogin(string username, string password)
    {
        long? userId = _userRepository.GetIdByCredentials(username, password);

        if (userId == null)
        {
            ErrorMessageTextBlock.Text = "Neispravno korisničko ime ili lozinka.";
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
            return;
        }

        Role? role = _userRepository.GetRoleById(userId.Value);
        if (role == null)
        {
            Close();
            return;
        }

        switch (role)
        {
            case Role.BuildingManager:
                var buildingManagerHomeWindow = new BuildingManager.HomeWindow();
                buildingManagerHomeWindow.Show();
                break;
            case Role.BuildingResident:
                var buildingResidentHomeWindow = new BuildingResident.HomeWindow(userId.Value);
                buildingResidentHomeWindow.Show();
                break;
            case Role.NeighborhoodCoordinator:
                break;
            case Role.Citizen:
                var citizenHomeWindow = new Citizen.HomeWindow();
                citizenHomeWindow.Show();
                break;
        }

        Close();
    }
}