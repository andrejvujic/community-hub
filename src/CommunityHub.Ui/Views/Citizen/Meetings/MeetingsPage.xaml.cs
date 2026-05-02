using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen.Meetings
{
    public partial class MeetingsPage : Page
    {
        private readonly long _userId;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly NeighborhoodMeetingRepository _neighborhoodMeetingRepository = new NeighborhoodMeetingRepository();
        private readonly MeetingDateVoteRepository _meetingDateVoteRepository = new MeetingDateVoteRepository();

        public MeetingsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            var citizen = _citizenRepository.GetById(_userId);
            DataContext = citizen;

            RefreshNeighborhoodMeetings();
        }

        private void RefreshNeighborhoodMeetings()
        {
            var citizen = _citizenRepository.GetById(_userId);
            if (citizen?.Membership != null)
            {
                var neighborhoodId = citizen.Membership.Neighborhood.Id;
                var meetings = _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId);
                MeetingsGrid.ItemsSource = meetings;
            }
        }

        private void MeetingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MeetingsGrid.SelectedItem is NeighborhoodMeeting selectedMeeting)
            {
                this.Tag = _meetingDateVoteRepository.GetByMeetingIdAndCitizenId(selectedMeeting.Id, _userId);
    }
            else
            {
                this.Tag = null;
            }
        }
    }
}