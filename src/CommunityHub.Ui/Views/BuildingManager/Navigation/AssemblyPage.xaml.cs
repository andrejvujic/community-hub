using System.Windows.Controls;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

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