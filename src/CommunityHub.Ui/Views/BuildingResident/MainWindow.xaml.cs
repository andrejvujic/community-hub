using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class MainWindow : Window, IMainNavigatorHost
    {
        private readonly long _userId;
        public MainWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
            MainFrame.Navigate(new HomePage(_userId));
        }

        public Frame HostFrame { get => MainFrame; }
    }
}
