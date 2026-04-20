using System.Windows.Controls;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class HomePage : Page
    {
        private readonly User _user;
        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
        }
    }
}