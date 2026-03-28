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
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text;
        string password = PasswordBox.Password;

        long? id = _userRepository.GetIdByCredentials(username, password);
        
        if (id == null)
        {
            ErrorMessageTextBlock.Text = "Neispravno korisničko ime ili lozinka.";
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
            return;
        }

        Role? role = _userRepository.GetRoleById(id.Value);
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
                var buildingResidentHomeWindow = new BuildingResident.HomeWindow();
                buildingResidentHomeWindow.Show();
                break;
            case Role.NeighborhoodCoordinator:
                break;
            case Role.Citizen:
                break;
        }

        Close();
    }
}