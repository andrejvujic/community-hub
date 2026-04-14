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
    public partial class MyBuildingsPage : Page
    {
        private readonly User _user;
        private readonly BuildingRepository _buildingRepository = new BuildingRepository();
        public ObservableCollection<Building> Buildings { get; } = new ObservableCollection<Building>();
        public MyBuildingsPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            _user = user;
        }
        private void MyBuildingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var buildings = _buildingRepository.GetByManagerId(_user.Id);
            Buildings.Clear();
            foreach (var building in buildings)
            {
                Buildings.Add(building);
            }
        }
        private void Details_Click(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            if (tb == null) return;

            var building = tb.DataContext as Building;
            if (building == null) return;

            var window = Window.GetWindow(this) as HomeWindow;
            if (window != null)
            {
                window.HomeFrame.Navigate(new BuildingDetailsPage(_user, building));
            }
        }
    }
}
