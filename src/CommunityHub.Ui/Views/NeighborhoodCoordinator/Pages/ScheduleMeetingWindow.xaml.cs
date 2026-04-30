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
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for ScheduleMeetingWindow.xaml
    /// </summary>
    public partial class ScheduleMeetingWindow : Window
    {
        private MeetingTheme? _theme;

        public ScheduleMeetingWindow(MeetingTheme? fixedTheme = null)
        {
            InitializeComponent();
            _theme = fixedTheme;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartVotingButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
