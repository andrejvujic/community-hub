using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly long _userId;
        private System.Windows.Controls.Border _selectedTab;
        private readonly SolidColorBrush _activeTabColor;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            var converter = new BrushConverter();
            _activeTabColor = (SolidColorBrush)converter.ConvertFromString("#99C2FF")!;

            Loaded += HomeWindow_Loaded;
            KeyDown += Window_KeyDown;

            MainFrame.Navigate(new MainMenuPage(_userId));
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
            if (sender is System.Windows.Controls.Border clickedTab)
            {
                ActivateTab(clickedTab);
            }
        }

        private void ActivateTab(System.Windows.Controls.Border newTab)
        {
            if (newTab == null || _selectedTab == newTab) 
                return;

            ChangeTabColorAndSelection(newTab);
            UpdateStatusBarForTab(newTab);
            NavigateToTab(newTab);
        }

        private void ChangeTabColorAndSelection(System.Windows.Controls.Border newTab)
        {
            if (_selectedTab != null)
            {
                _selectedTab.Background = Brushes.White;
            }

            _selectedTab = newTab;
            _selectedTab.Background = _activeTabColor;
        }

        private void UpdateStatusBarForTab(System.Windows.Controls.Border tab)
        {
            string displayName = tab.Uid ?? tab.Tag?.ToString() ?? "";

            if (StatusBarTabTextBlock != null)
            {
                StatusBarTabTextBlock.Text = $"Kartica: {displayName}";
            }
        }

        private void NavigateToTab(System.Windows.Controls.Border tab)
        {
            string tag = tab.Tag?.ToString() ?? "";

            switch (tag)
            {
                case "Main Menu":
                    MainFrame.Navigate(new MainMenuPage(_userId));
                    break;
                case "Neighborhoods":
                    MainFrame.Navigate(new NeighborhoodsPage(_userId));
                    break;
                case "Events":
                    MainFrame.Navigate(new EventsPage(_userId));
                    break;
                case "Trust Records":
                    MainFrame.Navigate(new TrustRecordsPage(_userId));
                    break;
                case "Meetings":
                    MainFrame.Navigate(new MeetingsPage(_userId));
                    break;
                case "Local Facilities":
                    MainFrame.Navigate(new LocalFacilitiesPage(_userId));
                    break;
                case "Donations":
                    MainFrame.Navigate(new DonationsPage(_userId));
                    break;
                default:
                    MainFrame.Content = null; 
                    break;
            }
        }
    }
}
