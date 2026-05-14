using CommunityHub.Ui.ViewModels.Citizen;
using CommunityHub.Ui.Views.Citizen.Donations;
using CommunityHub.Ui.Views.Citizen.Events;
using CommunityHub.Ui.Views.Citizen.LocalFacilities;
using CommunityHub.Ui.Views.Citizen.Meetings;
using CommunityHub.Ui.Views.Citizen.Neighborhoods;
using CommunityHub.Ui.Views.Citizen.TrustRecords;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class HomeWindow : Window
    {
        private readonly long _userId;
        private HomeViewModel? _viewModel;
        private SolidColorBrush? _activeTabColor;
        private SolidColorBrush? _inactiveTabColor;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += HomeWindow_Loaded;
            KeyDown += Window_KeyDown;
            Unloaded += HomeWindow_Unloaded;

            MainFrame.Navigate(new MainMenuPage(_userId));
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _activeTabColor = (SolidColorBrush)FindResource("ActiveTabBrush");
            _inactiveTabColor = Brushes.White;

            _viewModel = new HomeViewModel(_userId);
            _viewModel.Initialize(_activeTabColor!);
            DataContext = _viewModel;

            _viewModel.TabNavigationRequested += ViewModel_TabNavigationRequested;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void HomeWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.TabNavigationRequested -= ViewModel_TabNavigationRequested;
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            _viewModel = null;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HomeViewModel.SelectedTabTag))
            {
                UpdateTabVisuals(_viewModel?.SelectedTabTag ?? "");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.T)
            {
                TooltipToggleCheckBox.IsChecked = !TooltipToggleCheckBox.IsChecked;
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                e.Handled = true;
                return;
            }

            _viewModel?.HandleKeyDown(e.Key);
        }

        private void Tab_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border tab && tab.Tag is string tabTag && !string.IsNullOrEmpty(tabTag))
            {
                _viewModel?.HandleTabClick(tabTag);
            }
        }

        private void ViewModel_TabNavigationRequested(object? sender, TabNavigationEventArgs e)
        {
            NavigateToTab(e.TabTag);
        }

        private void UpdateTabVisuals(string selectedTabTag)
        {
            var tabs = new Dictionary<string, Border>
            {
                { "Main Menu", TabMainMenu },
                { "Neighborhoods", TabNeighborhoods },
                { "Events", TabEvents },
                { "Trust Records", TabTrustRecords },
                { "Meetings", TabMeetings },
                { "Local Facilities", TabLocalFacilities },
                { "Donations", TabDonations },
            };

            foreach (var (tag, border) in tabs)
            {
                border.Background = tag == selectedTabTag ? _activeTabColor : _inactiveTabColor;
            }
        }

        private void NavigateToTab(string tabTag)
        {
            Page? page = tabTag switch
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

            MainFrame.Navigate(page);
        }
    }
}
