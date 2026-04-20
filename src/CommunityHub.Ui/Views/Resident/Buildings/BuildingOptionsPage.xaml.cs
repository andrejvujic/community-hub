using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingOptionsPage : MainNavigatorConsumerPage
    {
        private readonly ImageRepository _imageRepository = new();

        public Building Building { get; }
        public BuildingOptionsPage(Building building)
        {
            InitializeComponent();
            DataContext = this;

            Building = building;
            GetBuildingImages();
        }

        void GetBuildingImages()
        {
            if (Building.HasImages) return;
            List<Image> images = _imageRepository.GetByBuildingId(Building.Id);
            foreach (var image in images)
                Building.Images.Add(image);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void OpenBuildingDetailsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new BuildingDetailsPage(Building));
    }
}
