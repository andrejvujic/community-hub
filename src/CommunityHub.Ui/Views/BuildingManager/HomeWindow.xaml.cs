using CommunityHub.Application.Database.Repositories;
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
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomeWindow : Window
    {
        private readonly long _userId;
        private Grid? _selected = null;
        private readonly UserRepository _userRepository = new UserRepository();
        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
            HomeFrame.Navigate(new HomePage(_userId));
        }
        private void NavBarItem_Click(object sender, MouseButtonEventArgs e)
        {
            var clicked = sender as Grid;
            if (clicked == null)
                return;

            if (!string.IsNullOrEmpty(clicked.Name) && clicked.Name.StartsWith("NavBar_"))
            {
                SetNavBarItemBackground(clicked);
            }

            string? tag = clicked.Tag?.ToString();
            switch (tag)
            {
                case "Home":
                    HomeFrame.Navigate(new HomePage(_userId));
                    break;
                case "Assembly":
                    HomeFrame.Navigate(new AssemblyPage(_userId));
                    break;
                case "MyBuildings":
                    HomeFrame.Navigate(new MyBuildingsPage(_userId));
                    break;
                case "Registration":
                    HomeFrame.Navigate(new RegistrationPage(_userId));
                    break;
                case "Help":
                    break;
                case "LogOut":
                    LogOutMessageBox();
                    break;
            }
        }
        private void HomeWindow_Loaded(object sender, RoutedEventArgs e) => SetUserName();
        private void SetUserName()
        {
            User? user = _userRepository.GetById(_userId);
            if (user == null)
                return;

            userNameTextBlock.Text = $"{user.Name}";
        }
        private void LogOutMessageBox()
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da se odjavite?", "Pažnja", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LogInForm logInForm = new LogInForm();
                logInForm.Show();
                this.Close();
            }
        }
        private void SetNavBarItemBackground(Grid item)
        {
            if (_selected != null)
                _selected.ClearValue(BackgroundProperty);
            _selected = item;
            _selected.Background = Brushes.LightGray;
        }    
    }
}