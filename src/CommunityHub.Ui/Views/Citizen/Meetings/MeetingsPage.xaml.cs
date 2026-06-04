using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.Meetings;

namespace CommunityHub.Ui.Views.Citizen.Meetings
{
    public partial class MeetingsPage : Page
    {
        private MeetingsViewModel? _viewModel;

        public MeetingsPage(long userId)
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                _viewModel = new MeetingsViewModel(userId);
                _viewModel.VoteRequested += (_, _) => HandleVote();
                DataContext = _viewModel;
            };
        }

        private void HandleVote()
        {
            var validationError = _viewModel?.ValidateVote();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selected = _viewModel?.SelectedMeeting;
            var wnd = new VoteForMeetingDateWindow(selected!);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                _viewModel?.ProcessVote(selected.Id, wnd.SelectedDate);
                MessageBox.Show("Vaš glas je uspešno zabeležen.", "Uspeh");
            }
        }
    }
}
