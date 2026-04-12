using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MainMenuPage : Page
    {
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly long _userId;
        private CommunityHub.Application.Domain.Citizen _citizen;
        private DispatcherTimer _timer;

        public MainMenuPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += MainMenuPage_Loaded;
            Unloaded += MainMenuPage_Unloaded;

            _timer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                (s, e) => UpdateDateTime(),
                Dispatcher.CurrentDispatcher);
        }

        private void MainMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUser();
            UpdateDateTime();
        }

        private void MainMenuPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
        }

        private void LoadUser()
        {
            _citizen = _citizenRepository.GetById(_userId);

            if (_citizen == null)
            {
                MessageBox.Show("Korisnik nije pronađen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataContext = _citizen;
        }

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now;
            var culture = new System.Globalization.CultureInfo("sr-Latn-RS");
            DateTextBlock.Text = now.ToString("dddd, d.M.yyyy.", culture);
            TimeTextBlock.Text = now.ToString("HH:mm", culture);
        }
    }
}
