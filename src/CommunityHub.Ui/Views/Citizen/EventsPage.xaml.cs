using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class EventsPage : Page
    {
        private readonly long _userId;

        public EventsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}
