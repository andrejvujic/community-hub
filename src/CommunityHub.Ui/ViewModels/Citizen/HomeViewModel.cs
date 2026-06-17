using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.Citizen;
using CommunityHub.Ui.Views.Citizen.Donations;
using CommunityHub.Ui.Views.Citizen.Events;
using CommunityHub.Ui.Views.Citizen.LocalFacilities;
using CommunityHub.Ui.Views.Citizen.Meetings;
using CommunityHub.Ui.Views.Citizen.Neighborhoods;
using CommunityHub.Ui.Views.Citizen.TrustRecords;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommunityHub.Ui.ViewModels.Citizen
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly CitizenService _citizenService = new();
        private readonly CommunityEventService _communityEventService = new();
        private readonly long _userId;
        private CitizenDto? _currentUser;
        private string _statusBarText = "Kartica: Glavni meni";
        private string _selectedTabTag = "Main Menu";
        private bool _tooltipEnabled = true;
        private SolidColorBrush? _activeTabColor;
        private SolidColorBrush? _inactiveTabColor;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<TabNavigationEventArgs>? TabNavigationRequested;
        public event EventHandler? CloseRequested;

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

        public string StatusBarText
        {
            get => _statusBarText;
            set
            {
                if (_statusBarText == value) return;
                _statusBarText = value;
                OnPropertyChanged(nameof(StatusBarText));
            }
        }

        public string SelectedTabTag
        {
            get => _selectedTabTag;
            set
            {
                if (_selectedTabTag == value) return;
                _selectedTabTag = value;
                OnPropertyChanged(nameof(SelectedTabTag));
            }
        }

        public bool TooltipEnabled
        {
            get => _tooltipEnabled;
            set
            {
                if (_tooltipEnabled == value) return;
                _tooltipEnabled = value;
                OnPropertyChanged(nameof(TooltipEnabled));
                TooltipState.IsEnabled = value;
            }
        }

        public SolidColorBrush? ActiveTabColor
        {
            get => _activeTabColor;
            set
            {
                if (_activeTabColor == value) return;
                _activeTabColor = value;
                OnPropertyChanged(nameof(ActiveTabColor));
            }
        }

        public SolidColorBrush? InactiveTabColor
        {
            get => _inactiveTabColor;
            set
            {
                if (_inactiveTabColor == value) return;
                _inactiveTabColor = value;
                OnPropertyChanged(nameof(InactiveTabColor));
            }
        }

        public ICommand SelectTabCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand ToggleTooltipCommand { get; }

        public static HomeViewModel CreateWithResources(long userId, Window window)
        {
            var activeTabColor = (SolidColorBrush)window.FindResource("ActiveTabBrush");
            var inactiveTabColor = (SolidColorBrush)window.FindResource("BackgroundBrush");
            return new HomeViewModel(userId, activeTabColor, inactiveTabColor);
        }

        public HomeViewModel(long userId, SolidColorBrush? activeTabColor = null, SolidColorBrush? inactiveTabColor = null)
        {
            _userId = userId;
            _activeTabColor = activeTabColor;
            _inactiveTabColor = inactiveTabColor;

            SelectTabCommand = new RelayCommand<string?>(execute: (tabTag) => HandleTabClick(tabTag ?? ""), canExecute: (tabTag) => !string.IsNullOrEmpty(tabTag));
            CloseWindowCommand = new RelayCommand<object>(execute: (_) => CloseRequested?.Invoke(this, EventArgs.Empty), canExecute: (_) => true);
            ToggleTooltipCommand = new RelayCommand<object>(execute: (_) => TooltipEnabled = !TooltipEnabled, canExecute: (_) => true);

            CurrentUser = _citizenService.GetCitizenById(_userId);
            UpdateEventStatusesIfReady();
        }

        public void HandleTabClick(string tabTag)
        {
            if (string.IsNullOrEmpty(tabTag) || SelectedTabTag == tabTag) return;

            SelectedTabTag = tabTag;
            StatusBarText = $"Kartica: {GetTabDisplayName(tabTag)}";
            var page = GetPageForTab(tabTag);
            TabNavigationRequested?.Invoke(this, new TabNavigationEventArgs { TabTag = tabTag, Page = page });
        }

        public Page? GetPageForTab(string tabTag)
        {
            return tabTag switch
            {
                "Main Menu" => new MainMenuPage(_userId),
                "Neighborhoods" => new NeighborhoodsPage(_userId),
                "Events" => new EventsPage(_userId),
                "Trust Records" => new TrustRecordsPage(_userId),
                "Meetings" => new MeetingsPage(_userId),
                "Local Facilities" => new LocalFacilitiesPage(_userId),
                "Donations" => new DonationsPage(_userId),
                _ => null,
            };
        }

        private string GetTabDisplayName(string tabTag) =>
            tabTag switch
            {
                "Main Menu" => "Glavni meni",
                "Neighborhoods" => "Kvartovi",
                "Events" => "Događaji",
                "Trust Records" => "Evidencije poverenja",
                "Meetings" => "Sastanci",
                "Local Facilities" => "Lokalni objekti",
                "Donations" => "Donacije",
                _ => tabTag,
            };

        private void UpdateEventStatusesIfReady()
        {
            if (CurrentUser?.Membership == null) return;

            Task.Run(() => { _communityEventService.UpdateEventStatusesForNeighborhood(CurrentUser.Membership.Neighborhood.Id); });
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TabNavigationEventArgs : EventArgs
    {
        public string TabTag { get; set; } = "";
        public Page? Page { get; set; }
    }
}
