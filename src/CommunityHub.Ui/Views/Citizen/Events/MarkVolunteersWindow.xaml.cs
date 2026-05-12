using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MarkVolunteersWindow : Window
    {
        private CommunityEvent _event;
        private readonly EventRegistrationRepository _eventRegistrationRepository = new EventRegistrationRepository();

        public MarkVolunteersWindow(CommunityEvent communityEvent)
        {
            InitializeComponent();
            _event = communityEvent;
            DataContext = _event;
        }

        private void PresenceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is EventRegistration reg)
            {
                bool newValue = cb.IsChecked == true;
                _eventRegistrationRepository.UpdateAttendance(reg.Id, newValue);
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            foreach (var reg in _event.Registrations)
            {
                if (reg.Attended == null)
                {
                    _eventRegistrationRepository.UpdateAttendance(reg.Id, false);
                }
            }
        }
    }
}
