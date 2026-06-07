using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.ViewModels.Citizen.Meetings;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Citizen.Meetings
{   
    public partial class VoteForMeetingDateWindow : Window
    {
        private readonly VoteForMeetingDateViewModel _viewModel;

        public DateOnly SelectedDate => _viewModel.SelectedDateAsDateOnly;

        public VoteForMeetingDateWindow(NeighborhoodMeetingDto meeting)
        {
            InitializeComponent();

            _viewModel = new VoteForMeetingDateViewModel(meeting);
            _viewModel.ConfirmRequested += (_, _) => { this.DialogResult = true; };
            _viewModel.CancelRequested += (_, _) => { this.DialogResult = false; };
            _viewModel.ToggleCalendarRequested += (_, _) => CalendarPopup.IsOpen = !CalendarPopup.IsOpen;

            DataContext = _viewModel;
            Loaded += (s, e) =>  { VoteDateTextBox.Focus(); VoteDateTextBox.SelectAll(); };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void VoteDateTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _viewModel.ParseAndSetDate(VoteDateTextBox.Text);
        }

        private void VoteCalendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalendarPopup.IsOpen = false;
        }
    }
}
