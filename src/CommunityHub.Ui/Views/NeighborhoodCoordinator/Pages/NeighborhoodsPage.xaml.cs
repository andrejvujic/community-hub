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
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for NeighborhoodsPage.xaml
    /// </summary>
    public partial class NeighborhoodsPage : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();
        private readonly NeighborhoodMembershipRepository _membershipRepository = new NeighborhoodMembershipRepository();

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();

        public NeighborhoodsPage()
        {
            InitializeComponent();
            DataContext = this;
            this.IsVisibleChanged += NeighborhoodsPage_IsVisibleChanged;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var n in neighborhoods)
            {
                Neighborhoods.Add(n);
            }
        }

        private void NeighborhoodsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshCurrentSelection();
            }
        }

        private void RefreshCurrentSelection()
        {
            if (NeighborhoodsListBox.SelectedItem is Neighborhood selected)
            {
                StreetsDataGrid.ItemsSource = _streetSectionRepository.GetByNeighborhoodId(selected.Id);
                MembershipsDataGrid.ItemsSource = _membershipRepository.GetCitizensByNeighborhoodId(selected.Id);
            }
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeighborhoodsListBox.SelectedItem is Neighborhood selected)
            {
                DetailsScrollViewer.DataContext = selected;
                RefreshCurrentSelection();
                DetailsScrollViewer.Visibility = Visibility.Visible;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterNeighborhoodWindow registerNeighborhoodWindow = new RegisterNeighborhoodWindow(_coordinatorId);
            registerNeighborhoodWindow.Owner = Window.GetWindow(this);
            
            if (registerNeighborhoodWindow.ShowDialog() == true)
            {
                LoadNeighborhoods();
            }
        }
    }
}
