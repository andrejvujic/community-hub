using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly UserRepository _userRepository;
        private readonly long _userId;
        private User _user;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userRepository = new UserRepository();
            _userId = userId;
            LoadUser();

            RequestsTab.Initialize(_userId);
        }

        private void LoadUser()
        {
            User? user = _userRepository.GetById(_userId);
            if (user == null)
            {
                MessageBox.Show("Korisnik nije pronađen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            _user = user;
            UserInfoTextBlock.Text = $"{_user.Name} {_user.Surname} - Koordinator kvarta";
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            loginForm.Show();
            this.Close();
        }
    }
}
