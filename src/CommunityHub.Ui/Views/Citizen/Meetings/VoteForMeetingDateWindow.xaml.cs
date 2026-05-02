using CommunityHub.Application.Domain;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.Meetings
{   
    public partial class VoteForMeetingDateWindow : Window
    {
        public VoteForMeetingDateWindow(NeighborhoodMeeting meeting)
        {
            InitializeComponent();
            DataContext = meeting;

            VoteDatePicker.DisplayDateStart = meeting.DateRangeStart.ToDateTime(TimeOnly.MinValue);
            VoteDatePicker.DisplayDateEnd = meeting.DateRangeEnd.ToDateTime(TimeOnly.MinValue);
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
