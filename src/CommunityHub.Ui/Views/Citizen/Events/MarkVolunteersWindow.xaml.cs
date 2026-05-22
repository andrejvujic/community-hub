using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MarkVolunteersWindow : Window
    {
        private readonly MarkVolunteersViewModel _viewModel;

        public MarkVolunteersWindow(CommunityEventDto eventDto)
        {
            InitializeComponent();
            _viewModel = new MarkVolunteersViewModel(eventDto);
            DataContext = _viewModel;
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PresenceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is EventRegistrationDto reg)
            {
                bool newValue = cb.IsChecked == true;
                _viewModel.UpdateAttendance(reg.Id, newValue);
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            _viewModel.MarkUnattendedAsAbsent();
        }
    }
}

