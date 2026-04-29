using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository = new();
        private readonly ImageRepository _imageRepository = new();

        public ObservableCollection<BuildingMembershipGroup> MembershipGroups { get; set; } = [];
        public bool HasMemberships { get => MembershipGroups.Count > 0; }

        public BuildingsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;

            UpdateMemberships();
        }

        public void LoadBuildingImages()
        {
            foreach (var building in MembershipGroups.Select(group => group.Building))
            {
                List<Image> images = _imageRepository.GetByBuilding(building.Id);
                foreach (var image in images)
                    building.Images.Add(image);
            }
        }

        public static List<BuildingMembershipGroup> GroupMembershipsByBuildingId(List<ApartmentMembership> memberships)
        {
            var groups = memberships.GroupBy(membership => membership.Apartment.Building.Id);
            return groups.Select(memberships =>
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
            List<ApartmentMembership> memberships = _apartmentMembershipRepository.GetByResident(_userId);
            List<BuildingMembershipGroup> groups = GroupMembershipsByBuildingId(memberships);

            MembershipGroups.Clear();
            foreach (var group in groups)
                MembershipGroups.Add(group);

            LoadBuildingImages();

            OnPropertyChanged(nameof(HasMemberships));
        }

        private void CreateAppartmentAccessRequestButton_Click(object sender, RoutedEventArgs e)
            => Push(new SearchBuildingsPage(_userId));

        private void OpenBuildingOptionsPageButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not BuildingMembershipGroup group) return;

            Push(new BuildingOptionsPage(
                userId: _userId,
                building: group.Building
            ));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
