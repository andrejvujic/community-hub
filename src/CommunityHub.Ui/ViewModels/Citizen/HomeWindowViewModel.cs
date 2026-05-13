using CommunityHub.Application;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using System.ComponentModel;
using System.Windows.Media;

namespace CommunityHub.Ui.ViewModels.Citizen
{
    public class HomeWindowViewModel : INotifyPropertyChanged
    {
        private readonly CitizenService _citizenService;
        private readonly CommunityEventService _communityEventService;
        private readonly long _userId;
        private CitizenDto? _currentUser;
        private string _statusBarText = "Kartica: Glavni meni";
        private string _selectedTabTag = "Main Menu";

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<TabNavigationEventArgs>? TabNavigationRequested;

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

        public HomeWindowViewModel(long userId)
        {
            _userId = userId;
            var citizenRepository = Injector.CreateInstance<ICitizenRepository>();
            var eventRepository = Injector.CreateInstance<ICommunityEventRepository>();

            _citizenService = new CitizenService(citizenRepository);
            _communityEventService = new CommunityEventService(eventRepository);

            LoadCurrentUser();
            UpdateEventStatusesIfReady();
        }

        public void Initialize(SolidColorBrush activeTabColor)
        {
            LoadCurrentUser();
            UpdateEventStatusesIfReady();
        }

        public void HandleTabClick(string tabTag)
        {
            if (string.IsNullOrEmpty(tabTag) || SelectedTabTag == tabTag)
                return;

            SelectedTabTag = tabTag;
            StatusBarText = $"Kartica: {GetTabDisplayName(tabTag)}";
            TabNavigationRequested?.Invoke(this, new TabNavigationEventArgs { TabTag = tabTag });
        }

        public void HandleKeyDown(System.Windows.Input.Key key)
        {
            var tabTag = key switch
            {
                System.Windows.Input.Key.F1 => "Main Menu",
                System.Windows.Input.Key.F2 => "Neighborhoods",
                System.Windows.Input.Key.F3 => "Events",
                System.Windows.Input.Key.F4 => "Trust Records",
                System.Windows.Input.Key.F5 => "Meetings",
                System.Windows.Input.Key.F6 => "Local Facilities",
                System.Windows.Input.Key.F7 => "Donations",
                _ => null,
            };

            if (tabTag != null)
                HandleTabClick(tabTag);
        }

        private void LoadCurrentUser()
        {
            CurrentUser = _citizenService.GetCitizenById(_userId);
        }

        private string GetTabDisplayName(string tabTag) =>
            tabTag switch
            {
                "Main Menu" => "Glavni meni",
                "Neighborhoods" => "Kvartovi",
                "Events" => "Događaji",
                "Trust Records" => "Rekord povjerenja",
                "Meetings" => "Sastanci",
                "Local Facilities" => "Lokalni objekti",
                "Donations" => "Donacije",
                _ => tabTag,
            };

        private void UpdateEventStatusesIfReady()
        {
            if (CurrentUser?.Membership == null)
                return;

            Task.Run(() =>
            {
                _communityEventService.UpdateEventStatusesForNeighborhood(CurrentUser.Membership.Neighborhood.Id);
            });
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TabNavigationEventArgs : EventArgs
    {
        public string TabTag { get; set; } = "";
    }
}
