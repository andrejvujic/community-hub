using System.Windows;
using System.Windows.Controls;
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
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedNeighborhood = NeighborhoodsListBox.SelectedItem as NeighborhoodDto;
        }

        private void MeetingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedMeeting = MeetingsDataGrid.SelectedItem as NeighborhoodMeetingDto;

            if (_viewModel.HasTiedDates)
                MessageBox.Show(
                    "Postoji izjednačen broj glasova. Odaberite jedan od termina.",
                    "Izjednačeni termini",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
        }

        private void ConfirmTermButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmTiedDate();
            MessageBox.Show("Termin je uspešno zakazan.", "Uspeh",
                MessageBoxButton.OK, MessageBoxImage.Information);
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
