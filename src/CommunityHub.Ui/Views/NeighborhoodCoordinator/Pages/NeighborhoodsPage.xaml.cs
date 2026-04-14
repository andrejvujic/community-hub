using System;
using System.Collections.Generic;
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
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for NeighborhoodsPage.xaml
    /// </summary>
    public partial class NeighborhoodsPage : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodRepository _neighborhoodRepository;
        private readonly StreetSectionRepository _streetSectionRepository;
        private readonly NeighborhoodMembershipRepository _membershipRepository;

        public NeighborhoodsPage()
        {
            InitializeComponent();
            _neighborhoodRepository = new NeighborhoodRepository();
            _streetSectionRepository = new StreetSectionRepository();
            _membershipRepository = new NeighborhoodMembershipRepository();
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            NeighborhoodsListBox.ItemsSource = neighborhoods;
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeighborhoodsListBox.SelectedItem is Neighborhood selected)
            {
                DetailsScrollViewer.DataContext = selected;
                SelectedNeighborhoodTitle.Text = selected.Name;
                SelectedNeighborhoodLocation.Text = $"{selected.City.Name}, {selected.Country.Name}";

                StreetsDataGrid.ItemsSource = _streetSectionRepository.GetByNeighborhoodId(selected.Id);
                MembershipsDataGrid.ItemsSource = _membershipRepository.GetCitizensByNeighborhoodId(selected.Id);

                DetailsScrollViewer.Visibility = Visibility.Visible;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterNeighborhoodWindow registerNeighborhoodWindow = new RegisterNeighborhoodWindow(_coordinatorId);
            registerNeighborhoodWindow.Owner = Window.GetWindow(this);
            registerNeighborhoodWindow.ShowDialog();
        }
    }
}
