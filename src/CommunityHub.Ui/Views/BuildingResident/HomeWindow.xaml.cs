using System.Windows;
using System.Windows.Input;


namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class HomeWindow : Window
    {
        private readonly long _userId;
        public HomeWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void BottomBar_RequestsItemClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new ApartmentAccessRequestsPage(_userId));
        }

        private void BottomBar_BuildingsItemClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new BuildingsPage());
        }
    }
}
