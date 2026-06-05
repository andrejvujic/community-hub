using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly HomeWindowViewModel _homeWindowViewModel = new();
        private readonly long _userId;

        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
            if (!_homeWindowViewModel.LoadUser(userId))
            {
                new InfoDialog("Greška", "Korisnik nije pronađen.", this).ShowDialog();
                Close();
                return;
            }

            UserInfoTextBlock.Text = _homeWindowViewModel.UserDisplayName;
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            NeighborhoodsTab.Initialize(_userId);
            RequestsTab.Initialize(_userId);
            ServiceCenterTab.Initialize(_userId);
            StatisticsAndMeetingsTab.Initialize(_userId);
            CityFacilitiesTab.Initialize(_userId);
            NeighborhoodBudgetTab.Initialize(_userId);
            ProfileTab.Initialize(_userId);
        }
 
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is not ComboBoxItem selected) return;

            string themePath = selected.Content.ToString() == "Tamna"
                ? "/Resources/Themes/DarkTheme.xaml"
                : "/Resources/Themes/LightTheme.xaml";

            var themeDict = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            var mergedDicts = System.Windows.Application.Current.Resources.MergedDictionaries;

            mergedDicts[0] = themeDict;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = new ConfirmDialog(
                "Potvrda odjave",
                "Da li ste sigurni da želite da se odjavite?",
                owner: this);

            if (confirmDialog.ShowDialog() != true) return;

            LogInForm loginForm = new LogInForm();
            loginForm.Show();
            this.Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key >= Key.D1 && e.Key <= Key.D8)
                {
                    MainTabControl.SelectedIndex = e.Key - Key.D1;
                    e.Handled = true;
                }
                else if (Keyboard.IsKeyDown(Key.Q))
                {
                    LogoutButton_Click(sender, e);
                }
                else if (Keyboard.IsKeyDown(Key.T))
                {
                    ThemeComboBox.SelectedIndex = ThemeComboBox.SelectedIndex == 0 ? 1 : 0;
                }
                else if (Keyboard.IsKeyDown(Key.L))
                {
                    LanguageComboBox.SelectedIndex = LanguageComboBox.SelectedIndex == 0 ? 1 : 0;
                }
            }
        }
    }
}
