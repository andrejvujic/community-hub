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

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for NeighborhoodsPage.xaml
    /// </summary>
    public partial class NeighborhoodsPage : UserControl
    {
        private long _coordinatorId;
        public NeighborhoodsPage()
        {
            InitializeComponent();
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }
    }
}
