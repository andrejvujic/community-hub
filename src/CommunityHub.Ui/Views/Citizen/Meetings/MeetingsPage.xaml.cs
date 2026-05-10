using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NeighborhoodMeetings;
using System.Windows;
using System.Windows.Controls;

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

        private void VoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = MeetingsGrid.SelectedItem as NeighborhoodMeeting;
            if (selected == null)
            {
                MessageBox.Show("Izaberite sastanak iz tabele.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (selected.Status != MeetingStatus.InPreparation)
            {
                MessageBox.Show("Može se glasati samo za sastanke koji su u statusu pripreme.", "Nije moguće glasanje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnd = new VoteForMeetingDateWindow(selected);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                ProcessVote(selected, wnd.SelectedDate);
            }
        }

        private void ProcessVote(NeighborhoodMeeting meeting, DateOnly selectedDate)
        {
            if (this.Tag is MeetingDateVote existingVote)
            {
                UpdateExistingVote(existingVote, selectedDate);
            }
            else
            {
                CreateNewVote(meeting, selectedDate);
            }

            RefreshNeighborhoodMeetings();
        }

        private void UpdateExistingVote(MeetingDateVote existingVote, DateOnly votedDate)
        {
            _meetingDateVoteRepository.UpdateDate(existingVote.Id, votedDate);
            MessageBox.Show("Vaš glas je uspešno izmenjen.", "Izmena");
        }

        private void CreateNewVote(NeighborhoodMeeting meeting, DateOnly votedDate)
        {
            var citizen = _citizenRepository.GetById(_userId);
            var newVote = new MeetingDateVote(meeting, citizen!, votedDate);
            _meetingDateVoteRepository.Create(newVote);
            MessageBox.Show("Vaš glas je uspešno zabeležen.", "Uspeh");
        }
    }
}