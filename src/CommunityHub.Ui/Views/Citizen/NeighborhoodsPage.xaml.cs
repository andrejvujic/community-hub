using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class NeighborhoodsPage : Page
    {
        private readonly long _userId;

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}
