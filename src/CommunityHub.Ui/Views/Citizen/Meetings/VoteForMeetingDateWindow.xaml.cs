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
            DataContext = _viewModel;
            VoteDateTextBox.TextChanged += (s, e) => _viewModel.ParseAndSetDate(VoteDateTextBox.Text);
            Loaded += (s, e) => 
            {
                VoteDateTextBox.Focus();
                VoteDateTextBox.SelectAll();
            };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = !CalendarPopup.IsOpen;
        }

        private void VoteCalendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalendarPopup.IsOpen = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedDate.HasValue)
                this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
