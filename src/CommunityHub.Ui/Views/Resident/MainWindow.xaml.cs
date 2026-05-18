using CommunityHub.Application.Database.Repositories;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Resident
{
    public partial class MainWindow : MainNavigatorHostWindow, IAlertHost
    {
        private readonly long _userId;
        public MainWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            MainFrame.Navigate(new HomePage(_userId));
        }

        public override Frame Frame => MainFrame;

        // IAlertHost:
        public FrameworkElement Root => AlertRoot;
        public TextBlock TypeTextBlock => AlertTypeTextBlock;
        public TextBlock TitleTextBlock => AlertTitleTextBlock;
        public TextBlock MessageTextBlock => AlertMessageTextBlock;
        public Panel OptionsPanel => AlertOptionsPanel;
    }
}
