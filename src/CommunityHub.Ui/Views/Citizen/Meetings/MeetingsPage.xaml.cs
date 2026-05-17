using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.ViewModels.Citizen.Meetings;

namespace CommunityHub.Ui.Views.Citizen.Meetings
{
    public partial class MeetingsPage : Page
    {
        private readonly long _userId;
        private readonly MeetingsViewModel _viewModel;

        public MeetingsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            _viewModel = new MeetingsViewModel(userId);
            DataContext = _viewModel;
        }

        private void MeetingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedMeeting = MeetingsGrid.SelectedItem as NeighborhoodMeetingDto;
        }

        private void VoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = _viewModel.SelectedMeeting;
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
                _viewModel.ProcessVote(selected.Id, wnd.SelectedDate);
                MessageBox.Show("Vaš glas je uspešno zabeležen.", "Uspeh");
            }
        }
    }
}