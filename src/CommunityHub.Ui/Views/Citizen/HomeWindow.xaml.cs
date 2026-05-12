using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Events;
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
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly long _userId;
        private Border? _selectedTab;
        private readonly SolidColorBrush _activeTabColor;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            _activeTabColor = (SolidColorBrush)FindResource("ActiveTabBrush");

            Loaded += HomeWindow_Loaded;
            KeyDown += Window_KeyDown;

            MainFrame.Navigate(new MainMenuPage(_userId));

            var citizen = _citizenRepository.GetById(_userId);
            if (citizen?.Membership != null)
            {
                Task.Run(() => UpdateEventStatusesIfReady(citizen.Membership.Neighborhood.Id));
            }
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var citizen = _citizenRepository.GetById(_userId);
            if (citizen != null)
            {
                DataContext = citizen;
            }
            _selectedTab = TabMainMenu;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.T)
            {
                if (TooltipToggleCheckBox != null)
                {
                    TooltipToggleCheckBox.IsChecked = !TooltipToggleCheckBox.IsChecked;
                }
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                // TODO: implement help system
                e.Handled = true;
                return;
            }

            switch (e.Key)
            {
                case Key.F1: ActivateTab(TabMainMenu); break;
                case Key.F2: ActivateTab(TabNeighborhoods); break;
                case Key.F3: ActivateTab(TabEvents); break;
                case Key.F4: ActivateTab(TabTrustRecords); break;
                case Key.F5: ActivateTab(TabMeetings); break;
                case Key.F6: ActivateTab(TabLocalFacilities); break;
                case Key.F7: ActivateTab(TabDonations); break;
            }
        }

        private void Tab_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border clickedTab)
            {
                ActivateTab(clickedTab);
            }
        }

        private void ActivateTab(Border newTab)
        {
            if (newTab == null || _selectedTab == newTab) 
                return;

            ChangeTabColorAndSelection(newTab);
            UpdateStatusBarForTab(newTab);
            NavigateToTab(newTab);
        }

        private void ChangeTabColorAndSelection(Border newTab)
        {
            if (_selectedTab != null)
            {
                _selectedTab.Background = Brushes.White;
            }

            _selectedTab = newTab;
            _selectedTab.Background = _activeTabColor;
        }

        private void UpdateStatusBarForTab(Border tab)
        {
            string displayName = tab.Uid ?? tab.Tag?.ToString() ?? "";

            if (StatusBarTabTextBlock != null)
            {
                StatusBarTabTextBlock.Text = $"Kartica: {displayName}";
            }
        }

        private void NavigateToTab(Border tab)
        {
            string tag = tab.Tag?.ToString() ?? string.Empty;

            Page? page = tag switch
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

            if (page is null)
                MainFrame.Content = null;
            else
                MainFrame.Navigate(page);
        }

        private void UpdateEventStatusesIfReady(long neighborhoodId)
        {
            var eventRepo = new CommunityEventRepository();
            var events = eventRepo.GetByNeighborhoodId(neighborhoodId);

            var preparationEvents = events.Where(e => e.Status == EventStatus.Preparation).ToList();
            foreach (var ev in preparationEvents)
            {
                var timeToEvent = ev.EventStart - DateTime.Now;
                bool enoughVolunteers = ev.Registrations.Count >= ev.MinVolunteers;
                bool allItemsAssigned = ev.Items.All(i => ev.Assignments.Any(a => a.Item.Id == i.Id));

                if (timeToEvent.TotalHours < 12 && (!enoughVolunteers || !allItemsAssigned))
                {
                    eventRepo.UpdateStatus(ev.Id, EventStatus.Cancelled);
                }
            }

            var scheduledEvents = events.Where(e => e.Status == EventStatus.Scheduled).ToList();
            foreach (var ev in scheduledEvents)
            {
                if (DateTime.Now > ev.EventStart.AddMinutes(ev.DurationMinutes))
                {
                    eventRepo.UpdateStatus(ev.Id, EventStatus.Finished);
                }
            }
        }
    }
}
