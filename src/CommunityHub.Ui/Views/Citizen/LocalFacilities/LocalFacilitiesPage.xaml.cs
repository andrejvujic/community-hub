using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class LocalFacilitiesPage : Page
    {
        public LocalFacilitiesPage(long userId)
        {
            InitializeComponent();
            DataContext = new LocalFacilitiesViewModel(userId);
        }
    }
}