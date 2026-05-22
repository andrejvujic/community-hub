using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MainMenuPage : Page
    {
        private MainMenuViewModel? _viewModel;

        public MainMenuPage(long userId)
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                _viewModel = new MainMenuViewModel(userId);
                DataContext = _viewModel;
            };
            Unloaded += (_, _) => _viewModel?.Cleanup();
        }
    }
}
