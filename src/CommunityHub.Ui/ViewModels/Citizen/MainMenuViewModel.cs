using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CommunityHub.Ui.ViewModels.Citizen
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private readonly CitizenService _citizenService = new();
        private readonly long _userId;
        private readonly DispatcherTimer _timer;
        private readonly CultureInfo _culture = new("sr-Latn-RS");

        private CitizenDto? _currentUser;
        private string _currentDate = "";
        private string _currentTime = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public CitizenDto? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value) return;
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                if (_currentDate == value) return;
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                if (_currentTime == value) return;
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public ICommand HelpWizardCommand { get; }
        public ICommand ShowHelpCommand { get; }


        public MainMenuViewModel(long userId)
        {
            _userId = userId;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;

            HelpWizardCommand = new RelayCommand<object>(_ => StartWizard());
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());

            CurrentUser = _citizenService.GetCitizenById(_userId);
            UpdateDateTime();
            _timer.Start();
        }

        private void StartWizard()
        {
            var steps = new List<WizardStep>
            {
                new WizardStep
                {
                    Title = "Dobrodošli u Community Hub!",
                    Description = "\n\n\n\n\nOvaj prozor će vas provesti kroz glavne funkcionalnosti aplikacije.\n\n\n" +
                    "Koristite F1-F7 tastere za brzo prebacivanje između kartica. Na svakoj kartici možete pronaći dodatne informacije i opcije."
                },
                new WizardStep
                {
                    Title = "Glavni meni",
                    Description = "Ovde vidite svoje osnovne informacije - ime, prezime, adresu i članstvo u kvartu. Odavde možete ponovo pokrenuti i sistem pomoći.",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w1.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Kvartovi",
                    Description = "Ovde vidite spisak svih kvartova i možete poslati zahtev za pristup. Možete i ocenjivati koordinatora svog kvarta.",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w2.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Događaji",
                    Description = "Ovde vidite informacije o svim dođajima u vašem kvartu. Možete se prijavljivati za volontiranje na događaje u pripremi ili organizovati nove događaje.",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w3.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Evidencije poverenja",
                    Description = "Ovde vidite informacije o svom statusu poverenja u kvartu. Statusi su: novi član (u kvartu kraće od godinu dana), neaktivan građanin (manje od 5 događaja), " +
                    "aktivan građanin (5+ događaja), istaknut građanin (10+ događaja), Građanin od poverenja (15+ događaja)",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w4.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Sastanci",
                    Description = "Ovde vidite informacije o svim sastancima u vašem kvartu. Možete glasati za datume sastanaka u pripremi.",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w5.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Lokalni objekti",
                    Description = "Ovde vidite informacije o lokalnim objektima u vašem kvartu. Možete glasati za posetu objekata, gledati istoriju i statistiku rezervacija.",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w6.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Donacije",
                    Description = "Ovde vidite budžet svojeg kvarta. Možete donirati novac kvartu za kategorije po vašem izboru",
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/HelpWizard/w7.png", UriKind.RelativeOrAbsolute))
                },
                new WizardStep
                {
                    Title = "Spremni ste!",
                    Description = "\n\n\n\n\n\n\nSada znate osnove. Istražite sve kartice i otkrijte šta sve Community Hub nudi!"
                }
            };

            var wizard = new HelpWizardWindow(steps);
            wizard.ShowDialog();
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Glavni meni",
                "Pregled vašeg profila i osnovnih informacija. Odavde možete pokrenuti sistem za pomoć.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+W", Description = "Pokreni sistem za pomoć" }
                }
            );
            helpWindow.ShowDialog();
        }

        public void Cleanup()
        {
            _timer.Tick -= Timer_Tick;
            _timer.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e) => UpdateDateTime();

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now;
            CurrentDate = now.ToString("dddd, d.M.yyyy.", _culture);
            CurrentTime = now.ToString("HH:mm", _culture);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
