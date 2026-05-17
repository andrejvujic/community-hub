using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.ViewModels.Citizen.Meetings;
using System.Windows;

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
