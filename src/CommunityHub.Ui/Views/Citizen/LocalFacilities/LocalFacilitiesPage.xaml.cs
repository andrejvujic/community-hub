using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class LocalFacilitiesPage : Page
    {
        private readonly long _userId;

        public LocalFacilitiesPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            DataContext = new LocalFacilitiesViewModel(_userId);
        }
    }
}