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
        private readonly long _userId;
        private readonly BuildingDbRepository _buildingRepository = new BuildingDbRepository();
        public ObservableCollection<Building> Buildings { get; } = new ObservableCollection<Building>();
        public MyBuildingsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;
            _userId = userId;
        }
        private void MyBuildingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var buildings = _buildingRepository.GetByManagerId(_userId);
            Buildings.Clear();
            foreach (var building in buildings)
            {
                Buildings.Add(building);
            }
        }
    }
}
