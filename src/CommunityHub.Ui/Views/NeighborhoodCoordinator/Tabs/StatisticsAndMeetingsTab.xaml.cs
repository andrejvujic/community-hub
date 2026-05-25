using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class StatisticsAndMeetingsTab : UserControl
    {
        private StatisticsAndMeetingsTabViewModel _viewModel = null!;

        public StatisticsAndMeetingsTab()
        {
            InitializeComponent();
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel = new StatisticsAndMeetingsTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadNeighborhoods();

            Window.GetWindow(this).KeyDown += StatisticsAndMeetingsTab_KeyDown;
        }

        private void StatisticsAndMeetingsTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsVisible) return;

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                && Keyboard.IsKeyDown(Key.N))
            {
                ScheduleNewMeetingButton_Click(sender, e);
            }
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedNeighborhood = NeighborhoodsListBox.SelectedItem as NeighborhoodDto;
        }

        private void MeetingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedMeeting = MeetingsDataGrid.SelectedItem as NeighborhoodMeetingDto;

            if (!_viewModel.HasTiedDates) return;

            Dispatcher.BeginInvoke(() =>
            {
                new InfoDialog("Izjednačeni termini", "Postoji izjednačen broj glasova. Odaberite jedan od termina.", Window.GetWindow(this)).ShowDialog();
            });
        }

        private void ConfirmTermButton_Click(object sender, RoutedEventArgs e)
        {
            var owner = Window.GetWindow(this);

            var confirmDialog = new ConfirmDialog(
                "Potvrda termina",
                "Da li ste sigurni da želite da potvrdite ovaj termin?",
                owner);

            confirmDialog.ShowDialog();

            if (!confirmDialog.Confirmed) return;

            _viewModel.ConfirmTiedDate();

            new InfoDialog("Uspeh", "Termin je uspešno zakazan.", owner).ShowDialog();
        }

        private void OpenScheduleWindow(MeetingTheme? theme)
        {
            if (_viewModel.SelectedNeighborhood is null) return;

            var window = new ScheduleMeetingWindow(_viewModel.SelectedNeighborhood, theme)
            {
                Owner = Window.GetWindow(this)
            };

            if (window.ShowDialog() == true)
                _viewModel.LoadMeetings();
        }

        private void WelcomeMeetingButton_Click(object sender, RoutedEventArgs e)
            => OpenScheduleWindow(MeetingTheme.Welcome);

        private void MotivationMeetingButton_Click(object sender, RoutedEventArgs e)
            => OpenScheduleWindow(MeetingTheme.Motivation);

        private void ScheduleNewMeetingButton_Click(object sender, RoutedEventArgs e)
            => OpenScheduleWindow(null);
    }
}
