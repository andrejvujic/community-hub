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
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for StatisticsAndMeetingsTab.xaml
    /// </summary>
    public partial class StatisticsAndMeetingsTab : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();

        public StatisticsAndMeetingsTab()
        {
            InitializeComponent();
            DataContext = this;
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
    }
}
