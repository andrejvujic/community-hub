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

            NeighborhoodsTab.Initialize(_userId);
            RequestsTab.Initialize(_userId);
            ServiceCenterTab.Initialize(_userId);
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
            if (ThemeComboBox.SelectedItem is not ComboBoxItem selected) return;

            string themePath = selected.Content.ToString() == "Tamna"
                ? "/Resources/Themes/DarkTheme.xaml"
                : "/Resources/Themes/LightTheme.xaml";

            var themeDict = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            var mergedDicts = System.Windows.Application.Current.Resources.MergedDictionaries;

            mergedDicts[0] = themeDict;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite da se odjavite?",
                "Potvrda odjave",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            LogInForm loginForm = new LogInForm();
            loginForm.Show();
            this.Close();
        }
    }
}
