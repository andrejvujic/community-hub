using System.Windows.Controls;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class AssemblyPage : Page
    {
        private readonly User _user;
        public AssemblyPage(User user)
        {
            InitializeComponent();
            _user = user;
        }
    }
}