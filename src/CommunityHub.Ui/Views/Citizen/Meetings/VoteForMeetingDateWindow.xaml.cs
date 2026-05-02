using CommunityHub.Application.Domain;
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

namespace CommunityHub.Ui.Views.Citizen.Meetings
{   
    public partial class VoteForMeetingDateWindow : Window
    {
        public VoteForMeetingDateWindow(NeighborhoodMeeting meeting)
        {
            InitializeComponent();
            DataContext = meeting;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
