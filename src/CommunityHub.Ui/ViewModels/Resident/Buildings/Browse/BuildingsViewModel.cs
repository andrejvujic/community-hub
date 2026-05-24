using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Browse
{
    public class BuildingsViewModel : BaseViewModel
    {
        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        public long UserId { get; }

        public BuildingsViewModel(long userId, IMainNavigationConsumer navigationConsumer) : base(navigationConsumer)
        {
            UserId = userId;
            OpenBuildingOptionsCommand = new(OpenBuildingOptions);
        }

        public RelayCommand<BuildingMembershipGroupDto> OpenBuildingOptionsCommand { get; }

        private void OpenBuildingOptions(BuildingMembershipGroupDto? group)
        {
            if (group is null) return;

            Push(new BuildingOptionsPage(
                userId: UserId,
                building: group.Building
            ));
        }

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
