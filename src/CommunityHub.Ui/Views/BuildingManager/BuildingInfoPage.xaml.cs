using System.Windows.Controls;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingInfoPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        public BuildingInfoPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
        }
    }
}