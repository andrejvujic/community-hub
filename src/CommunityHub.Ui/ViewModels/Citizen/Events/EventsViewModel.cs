using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService = new();
        private readonly EventRegistrationService _registrationService = new();
        private readonly EventReportService _reportService = new();
        private readonly CitizenService _citizenService = new();

        private CitizenDto? _citizen;
        private ObservableCollection<CommunityEventDto> _events = new();
        private object? _membership;
        private CommunityEventDto? _selectedEvent;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ApplyToVolunteerRequested;
        public event EventHandler? MyEventsRequested;
        public event EventHandler<string>? ShowInfoMessage;

        public CitizenDto? Citizen
        {
            get => _citizen;
            private set
            {
                if (_citizen == value) return;
                _citizen = value;
                OnPropertyChanged(nameof(Citizen));
            }
        }

        public ObservableCollection<CommunityEventDto> Events => _events;
        public bool CanPrintReport => SelectedEvent != null;

        public CommunityEventDto? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent == value) return;
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                OnPropertyChanged(nameof(CanApplyToVolunteer));
                OnPropertyChanged(nameof(ApplyToVolunteerTooltip));
            }
        }

        public bool CanApplyToVolunteer
        {
            get
            {
                if (SelectedEvent == null) return false;

                bool isInPreparation = SelectedEvent.Status == EventStatus.Preparation;
                bool isAlreadyRegistered = _registrationService.IsAlreadyRegistered(_citizen!.Id, SelectedEvent.Id);

                return isInPreparation && !isAlreadyRegistered;
            }
        }

        public string ApplyToVolunteerTooltip
        {
            get
            {
                if (!TooltipState.IsEnabled) return null;
                if (!CanApplyToVolunteer) return ApplyDisabledReason;
                return null;
            }
        }

        public string ApplyDisabledReason
        {
            get
            {
                if (SelectedEvent == null) return "Prvo morate izabrati događaj";
                if (SelectedEvent.Status != EventStatus.Preparation) return "Možete se prijaviti samo za događaje koji su u pripremi";
                return "Već ste prijavljeni za ovaj događaj";
            }
        }

        public object? Membership
        {
            get => _membership;
            private set
            {
                if (_membership == value) return;
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }

        public ICommand ApplyToVolunteerCommand { get; }
        public ICommand PrintReportCommand { get; }
        public ICommand MyEventsCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public EventsViewModel(long userId)
        {
            _userId = userId;

            ApplyToVolunteerCommand = new RelayCommand<object>(_ => { if (CanApplyToVolunteer) ApplyToVolunteerRequested?.Invoke(this, EventArgs.Empty); });
            PrintReportCommand = new RelayCommand<object>(_ => PrintReport());
            MyEventsCommand = new RelayCommand<object>(_ => MyEventsRequested?.Invoke(this, EventArgs.Empty));
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());

            TooltipState.OnPropertyChanged += (s, e) => { OnPropertyChanged(nameof(ApplyToVolunteerTooltip)); };

            LoadData();
        }

        private void PrintReport()
        {
            if (SelectedEvent == null)
            {
                MessageBox.Show("Molimo izaberite događaj za štampanje izveštaja.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var pdfData = _reportService.GenerateEventReport(SelectedEvent, $"{Citizen!.Name} {Citizen!.Surname}");

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF dokument (*.pdf)|*.pdf",
                    FileName = $"Izvestaj_{SelectedEvent.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Sačuvaj izveštaj",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveDialog.ShowDialog() == true)
                {
                    _reportService.SaveReportToFile(pdfData, saveDialog.FileName);

                    var result = MessageBox.Show($"Izveštaj je uspešno sačuvan.\n\nDa li želite da otvorite PDF?","Uspešno", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom generisanja izveštaja:\n\n{ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Događaji",
                "Pregledajte sve događaje u vašem kvartu. Možete se prijaviti za volontiranje, štampati izveštaje o događajima i pregledati svoje događaje. " +
                "Takođe, možete upravljati svojim događajima i pregledati informacije o volonterskim prijavama, kao i organizovati novi događaj.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+1", Description = "Prijavi se za volontiranje" },
                    new() { Keys = "Ctrl+2", Description = "Štampaj izveštaj o događaju" },
                    new() { Keys = "Ctrl+3", Description = "Pregledaj svoje događaje i volontiranja" },
                }
            );
            helpWindow.ShowDialog();
        }

        private void LoadData()
        {
            Citizen = _citizenService.GetCitizenById(_userId);

            if (_citizen?.Membership == null)
            {
                Membership = null;
                return;
            }

            RefreshEvents();
            Membership = _citizen.Membership;
        }

        public void RefreshEvents()
        {
            var selectedId = _selectedEvent?.Id;
            _events.Clear();

            if (_citizen?.Membership == null) return;

            var neighborhoodId = _citizen.Membership.Neighborhood.Id;
            var events = _eventService.GetByNeighborhoodId(neighborhoodId);

            foreach (var eventDto in events) _events.Add(eventDto);

            SelectedEvent = Events.FirstOrDefault(n => n.Id == selectedId) ?? Events.FirstOrDefault();
        }

        public long GetCitizenId() => _citizen?.Id ?? 0;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
