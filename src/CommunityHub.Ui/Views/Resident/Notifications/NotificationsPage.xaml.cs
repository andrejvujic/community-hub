using CommunityHub.Ui.ViewModels.Resident.Notifications;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications
{
    public partial class NotificationsPage : MainNavigationConsumerPage
    {
        public NotificationsPage(long userId)
        {
            InitializeComponent();
            DataContext = new NotificationsViewModel(userId, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
