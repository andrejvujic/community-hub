using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingOptionsPage : MainNavigatorConsumerPage
    {
        public Building Building { get; }
        public BuildingOptionsPage(Building building)
        {
            InitializeComponent();
            DataContext = this;

            Building = building;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void OpenBuildingDetailsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new BuildingDetailsPage(Building));
    }
}
