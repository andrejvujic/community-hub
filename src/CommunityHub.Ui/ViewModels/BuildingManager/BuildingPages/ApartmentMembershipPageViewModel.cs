using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Dtos.Apartments;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class ApartmentMembershipPageViewModel
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly ApartmentMembershipService _apartmentMembershipService = new();
        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public ObservableCollection<ApartmentMembershipDto> ApartmentMemberships { get; } = [];

        public string StreetInfo
        {
            get => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        }
        public ApartmentMembershipPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);
            LoadApartmentMemberships();
        }
        public void LoadApartmentMemberships()
        {
            ApartmentMemberships.Clear();
            var memberships = _apartmentMembershipService.GetByUserAndBuilding(User.Id, Building.Id);

            foreach (var membership in memberships)
                ApartmentMemberships.Add(membership);
        }
    }
}