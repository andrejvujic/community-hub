using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentMembershipsPage : Page
    {
        private readonly Building _building;
        private readonly User _user;
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository = new();
        public ObservableCollection<ApartmentMembership> ApartmentMemberships { get; set; } = new();
        public ApartmentMembershipsPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            SetStreetInfo();
            DataContext = this;
        }
        private void ApartmentMembershipsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApartmentMemberships.Clear();

            var memberships = _apartmentMembershipRepository.GetByUserAndBuilding(_user.Id, _building.Id);

            foreach (var membership in memberships)
                ApartmentMemberships.Add(membership);
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
    }
}
