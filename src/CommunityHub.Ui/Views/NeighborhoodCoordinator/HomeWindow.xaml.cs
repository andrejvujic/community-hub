using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator
{
    public partial class HomeWindow : Window
    {
        private readonly HomeWindowViewModel _viewModel;
        private readonly long _userId;

        public HomeWindow(long userId)
        {
            InitializeComponent();

            _userId = userId;
            _viewModel = new HomeWindowViewModel(userId);
            DataContext = _viewModel;

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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key >= Key.D1 && e.Key <= Key.D8)
                {
                    MainTabControl.SelectedIndex = e.Key - Key.D1;
                    e.Handled = true;
                }
                else if (e.Key == Key.Q)
                {
                    if (_viewModel.LogoutCommand.CanExecute(null))
                        _viewModel.LogoutCommand.Execute(null);
                }
                else if (e.Key == Key.T)
                {
                    _viewModel.SelectedThemeIndex = _viewModel.SelectedThemeIndex == 0 ? 1 : 0;
                }
                else if (e.Key == Key.L)
                {
                    _viewModel.SelectedLanguageIndex = _viewModel.SelectedLanguageIndex == 0 ? 1 : 0;
                }
            }
        }
    }
}