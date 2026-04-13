using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository;

        public ObservableCollection<BuildingMembershipGroup> MembershipGroups { get; set; } = [];
        public bool HasMemberships { get => MembershipGroups.Count > 0; }

        public BuildingsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;

            _apartmentMembershipRepository = new ApartmentMembershipRepository();

            UpdateMemberships();
        }

        public List<BuildingMembershipGroup> GroupMembershipsByBuildingId(List<ApartmentMembership> memberships)
        {
            return memberships
                    .GroupBy(membership => membership.Apartment.Building.Id)
                    .Select(memberships =>
                        new BuildingMembershipGroup(
                            memberships.First().Apartment.Building,
                            memberships.First().Resident
                        )
                        {
                            Memberships = [..memberships]
                        })
                    .ToList();
        }

        void UpdateMemberships()
        {
            List<ApartmentMembership> memberships = _apartmentMembershipRepository.GetByResidentId(_userId);
            List<BuildingMembershipGroup> groups = GroupMembershipsByBuildingId(memberships);

            MembershipGroups.Clear();
            foreach (var group in groups)
                MembershipGroups.Add(group);

            OnPropertyChanged(nameof(HasMemberships));
        }

        private void CreateAppartmentAccessRequestButton_Click(object sender, System.Windows.RoutedEventArgs e)
            => Push(new SearchBuildingsPage(_userId));

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
