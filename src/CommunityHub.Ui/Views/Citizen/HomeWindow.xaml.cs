using System;
using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly CitizenRepository _citizenRepository;
        private readonly long _userId;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _citizenRepository = new CitizenRepository();
            _userId = userId;

            Loaded += HomeWindow_Loaded;
            
            MainFrame.Navigate(new HomePage(_userId));
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var citizen = _citizenRepository.GetById(_userId);
            if (citizen != null)
            {
                DataContext = citizen;
            }
        }
    }
}
