using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Modules.Buildings.Core.Services.Apartments;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings
{
    public class BuildingsViewModel : INotifyPropertyChanged
    {
        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        public long UserId { get; }

        public BuildingsViewModel(long userId)
        {
            UserId = userId;
        }

        public ObservableCollection<BuildingMembershipGroup> MembershipGroups { get; set; } = new();
        public bool HasMemberships { get => MembershipGroups.Count > 0; }

        private static List<BuildingMembershipGroup> GroupMembershipsByBuilding(List<ApartmentMembership> memberships)
        {
            var groups = memberships.GroupBy(membership => membership.Apartment.Building.Id);
            return groups.Select(memberships =>
                new BuildingMembershipGroup(
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
