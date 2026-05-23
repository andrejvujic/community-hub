using System.Windows;
using System.Windows.Controls;
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
            var dialog = new ConfirmDialog(
                "Potvrda odjave",
                "Da li ste sigurni da želite da se odjavite?",
                owner: this);

            dialog.ShowDialog();

            if (!dialog.Confirmed) return;

            LogInForm loginForm = new LogInForm();
            loginForm.Show();
            this.Close();
        }
    }
}
