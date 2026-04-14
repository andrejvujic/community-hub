using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident
{
    public partial class HomePage : Page
    {
        private readonly long _userId;
        public HomePage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            OpenBuildingsPage();
        }

        private void OpenBuildingsPage() => HomeFrame.Navigate(new BuildingsPage(_userId));
        private void OpenApartmentAccessRequestsPage() => HomeFrame.Navigate(new ApartmentAccessRequestsPage(_userId));

        private void BottomBar_RequestsItemClick(object sender, MouseButtonEventArgs e) => OpenApartmentAccessRequestsPage();
        private void BottomBar_BuildingsItemClick(object sender, MouseButtonEventArgs e) => OpenBuildingsPage();
    }
}
