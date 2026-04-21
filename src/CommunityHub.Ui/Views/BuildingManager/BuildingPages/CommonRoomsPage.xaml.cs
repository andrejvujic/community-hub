using CommunityHub.Application.Domain;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomsPage : Page
    {
        private readonly Building _building;
        private readonly User _user;
        public CommonRoomsPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
        }
    }
}
