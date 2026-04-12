using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MeetingsPage : Page
    {
        private readonly long _userId;

        public MeetingsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}