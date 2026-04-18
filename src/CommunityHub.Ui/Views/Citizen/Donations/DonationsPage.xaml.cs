using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.Donations
{
    public partial class DonationsPage : Page
    {
        private readonly long _userId;

        public DonationsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}