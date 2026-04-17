using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class LocalFacilitiesPage : Page
    {
        private readonly long _userId;

        public LocalFacilitiesPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }
    }
}