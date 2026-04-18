using System.Windows.Controls;
using CommunityHub.Application.Domain;

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