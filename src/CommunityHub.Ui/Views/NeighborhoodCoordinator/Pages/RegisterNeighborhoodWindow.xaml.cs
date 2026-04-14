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
    /// Interaction logic for RegisterNeighborhoodWindow.xaml
    /// </summary>
    public partial class RegisterNeighborhoodWindow : Window
    {
        private readonly long _coordinatorId;
        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();
            _coordinatorId = coordinatorId;
        }
    }
}
