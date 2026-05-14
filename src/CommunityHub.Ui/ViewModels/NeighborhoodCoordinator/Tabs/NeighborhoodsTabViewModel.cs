using System.Collections.ObjectModel;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Images;
using CommunityHub.Application.Services.Neighborhoods;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class NeighborhoodsTabViewModel
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly StreetSectionService _streetSectionService = new();
        private readonly ImageService _imageService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; set; } = new();

        public NeighborhoodsTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
            {
                n.StreetSections = _streetSectionService.GetByNeighborhood(n.Id);
                n.Images = _imageService.GetByNeighborhood(n.Id);
                Neighborhoods.Add(n);
            }
        }
    }
}
