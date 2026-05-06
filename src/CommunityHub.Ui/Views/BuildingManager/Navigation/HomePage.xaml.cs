using System.Windows.Controls;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

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