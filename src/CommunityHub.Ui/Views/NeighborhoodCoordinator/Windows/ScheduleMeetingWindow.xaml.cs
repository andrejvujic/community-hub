using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ScheduleMeetingWindow : Window
    {
        private readonly ScheduleMeetingWindowViewModel _viewModel;

        public ScheduleMeetingWindow(NeighborhoodDto neighborhood, MeetingTheme? fixedTheme = null)
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new ScheduleMeetingWindowViewModel(neighborhood, fixedTheme);
            DataContext = _viewModel;

            FromDatePicker.DisplayDateStart = _viewModel.MinDate;
            FromDatePicker.DisplayDateEnd = _viewModel.MaxDate;

            ToDatePicker.DisplayDateStart = _viewModel.MinDate;
            ToDatePicker.DisplayDateEnd = _viewModel.MaxDate;

            if (!_viewModel.HasFixedTheme)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void StartVotingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.HasFixedTheme &&
                ThemeComboBox.SelectedItem is ComboBoxItem item &&
                item.Tag is MeetingTheme theme)
                _viewModel.SelectedTheme = theme;

            var (success, error) = _viewModel.TryCreate();
            if (!success)
            {
                MessageBox.Show(error, "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}