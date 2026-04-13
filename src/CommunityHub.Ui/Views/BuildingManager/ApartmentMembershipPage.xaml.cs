using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentMembershipPage : Page
    {
        private readonly Building _building;
        private readonly User _user;
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository = new ApartmentMembershipRepository();
        public ObservableCollection<ApartmentMembership> ApartmentMemberships { get; set; } = new ObservableCollection<ApartmentMembership>();
        public ApartmentMembershipPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            SetStreetInfo();
            DataContext = this;
        }
        private void LeftArrow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;
            if (homeWindow != null)
            {
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
            }
        }
        private void SetStreetInfo()
        {
            streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        }
        private void LoadApartmentMemberships(object sender, RoutedEventArgs e)
        {
            ApartmentMemberships.Clear();
            var requests = _apartmentMembershipRepository.GetByUserAndBuilding(_user.Id, _building.Id);
            foreach (var request in requests)
            {
                ApartmentMemberships.Add(request);
            }
        }
    }
}
