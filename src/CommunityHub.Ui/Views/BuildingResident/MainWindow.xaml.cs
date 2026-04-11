using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class MainWindow : MainNavigatorHostWindow
    {
        private readonly long _userId;
        public MainWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
            MainFrame.Navigate(new HomePage(_userId));
        }

        public override Frame Frame => MainFrame;
    }
}
