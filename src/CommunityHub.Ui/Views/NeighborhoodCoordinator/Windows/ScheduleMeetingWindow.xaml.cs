using System.Windows;
using System.Windows.Input;
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
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }
    }
}