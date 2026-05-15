using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MyEventsWindow : Window
    {
        private readonly MyEventsViewModel _viewModel;

        public MyEventsWindow(long userId)
        {
            InitializeComponent();
            _viewModel = new MyEventsViewModel(userId);
            this.DataContext = _viewModel;

            OrganizerEventsList.PreviewKeyDown += OrganizerEventsList_PreviewKeyDown;
        }

        private void OrganizeButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new OrganizeEventWindow(_viewModel.UserId);
            wnd.Owner = this;
            var dialogResult = wnd.ShowDialog();
            if (dialogResult == true)
            {
                _viewModel.RefreshAfterOrganizeWindow();
                this.DialogResult = true;
            }
        }

        private void OrganizerEventsList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var selectedEvent = OrganizerEventsList.SelectedItem as CommunityEventDto;

                if (!_viewModel.CanOpenMarkVolunteersWindow(selectedEvent))
                    return;

                var eventDto = _viewModel.GetEventDto(selectedEvent!.Id);
                if (eventDto == null)
                    return;

                var wnd = new MarkVolunteersWindow(eventDto);
                wnd.Owner = this;
                wnd.ShowDialog();
                e.Handled = true;

                _viewModel.RefreshAfterMarkVolunteersWindow();
            }
        }
    }
}

