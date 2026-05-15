using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MainMenuPage : Page
    {
        private readonly long _userId;
        private MainMenuViewModel? _viewModel;

        public MainMenuPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += MainMenuPage_Loaded;
            Unloaded += MainMenuPage_Unloaded;
        }

        private void MainMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new MainMenuViewModel(_userId);
            DataContext = _viewModel;
        }

        private void MainMenuPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel?.Cleanup();
        }
    }
}
