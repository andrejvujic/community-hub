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

        public NeighborhoodsPage()
        {
            InitializeComponent();
            _neighborhoodRepository = new NeighborhoodRepository();
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            NeighborhoodsItemsControl.ItemsSource = neighborhoods;
        }

        public void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void MembershipsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
