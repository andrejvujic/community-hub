using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Browse
{
    public class BuildingsViewModel : BaseViewModel
    {
        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        public long UserId { get; }

        public BuildingsViewModel(long userId, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
        }

        public void OpenSearchBuildingsPage()
            => Push(new SearchBuildingsPage(UserId));

        public void OpenBuildingOptionsPage(BuildingMembershipGroupDto group)
            => Push(new BuildingOptionsPage(UserId, group.Building));

        public ObservableCollection<BuildingMembershipGroupDto> MembershipGroups { get; set; } = new();
        public bool HasMemberships { get => MembershipGroups.Count > 0; }

        private static List<BuildingMembershipGroupDto> GroupMembershipsByBuilding(
            List<ApartmentMembershipDto> memberships
        )
        {
            var groups = memberships.GroupBy(membership => membership.Apartment.Building.Id);
            return groups.Select(memberships =>
                new BuildingMembershipGroupDto(
                    memberships.First().Apartment.Building,
                    memberships.First().Resident
                )
                {
                    Memberships = [.. memberships]
                })
                .ToList();
        }

        public void UpdateMemberships()
        {
            var memberships = _apartmentMembershipService.GetMemberships(UserId);
            var groups = GroupMembershipsByBuilding(memberships);

            MembershipGroups.Clear();
            foreach (var group in groups)
                MembershipGroups.Add(group);

            OnPropertyChanged(nameof(HasMemberships));
        }

    }
}
