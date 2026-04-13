using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class TrustRecordsPage : Page
    {
        private readonly long _userId;

        public TrustRecordsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}