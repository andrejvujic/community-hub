using System.Windows;
using CommunityHub.Application.Database.Repositories.Shared;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.Views;

public partial class LogInForm : Window
{
    private readonly UserRepository _userRepository;

    public LogInForm()
    {
        InitializeComponent();
        _userRepository = new UserRepository();
        
        // Uncomment for auto login:
        // HandleLogin("stefan.matic", "123");
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

        User user = (_userRepository.Get(userId.Value))!;

        switch (role)
        {
            case Role.BuildingManager:
                var buildingManagerHomeWindow = new BuildingManager.HomeWindow(user);
                buildingManagerHomeWindow.Show();
                break;
            case Role.BuildingResident:
                var buildingResidentMainWindow = new Resident.MainWindow(userId.Value);
                buildingResidentMainWindow.Show();
                break;
            case Role.NeighborhoodCoordinator:
                var neighborhoodCoordinatorHomeWindow = new NeighborhoodCoordinator.HomeWindow(userId.Value);
                neighborhoodCoordinatorHomeWindow.Show();
                break;
            case Role.Citizen:
                var citizenHomeWindow = new Citizen.HomeWindow(userId.Value);
                citizenHomeWindow.Show();
                break;
            /*
            case Role.NeighborhoodServiceProvider:
                var serviceProviderHomeWindow = new ServiceProvider.HomeWindow(userId.Value);
                serviceProviderHomeWindow.Show();
                break;
            */
        }

        Close();
    }
}