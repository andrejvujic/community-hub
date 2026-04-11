using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class AssemblyPage : Page
    {
        private readonly long _userId;
        public AssemblyPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}
