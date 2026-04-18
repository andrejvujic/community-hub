using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Globalization;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MainMenuPage : Page
    {
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly long _userId;

        private DispatcherTimer _timer;
        private static readonly CultureInfo culture = new CultureInfo("sr-Latn-RS");

        public MainMenuPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += MainMenuPage_Loaded;
            Unloaded += MainMenuPage_Unloaded;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void MainMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUser();
            UpdateDateTime();
        }

        private void MainMenuPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Tick -= Timer_Tick;
                _timer.Stop();
                _timer = null!;
            }
        }

        private void LoadUser()
        {
            var citizen = _citizenRepository.GetById(_userId);

            if (citizen == null)
            {
                MessageBox.Show("Korisnik nije pronađen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataContext = citizen;
        }

        private void Timer_Tick(object? sender, EventArgs e) => UpdateDateTime();

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now;
            DateTextBlock.Text = now.ToString("dddd, d.M.yyyy.", culture);
            TimeTextBlock.Text = now.ToString("HH:mm", culture);
        }
    }
}
