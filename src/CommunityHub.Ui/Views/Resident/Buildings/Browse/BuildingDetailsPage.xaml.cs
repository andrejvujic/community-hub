using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Browse;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingDetailsPage : MainNavigationConsumerPage
    {
        public BuildingDetailsPage(BuildingDto building)
        {
            InitializeComponent();
            DataContext = new BuildingDetailsViewModel(building);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
    }
}
