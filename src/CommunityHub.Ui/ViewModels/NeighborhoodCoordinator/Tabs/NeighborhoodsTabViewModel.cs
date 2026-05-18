using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Images;
using CommunityHub.Application.Services.Neighborhoods;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class NeighborhoodsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly StreetSectionService _streetSectionService = new();
        private readonly ImageService _imageService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<StreetSectionDto> NeighborhoodStreetSections { get; } = new();
        public ObservableCollection<CitizenDto> NeighborhoodMembers { get; } = new();

        private NeighborhoodDto? _selectedNeighborhood;
        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDetailVisible));
                LoadDetails();
            }
        }

        public bool IsDetailVisible => SelectedNeighborhood != null;

        public NeighborhoodsTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }

        public void LoadNeighborhoods()
        {
            var previously = SelectedNeighborhood;
            Neighborhoods.Clear();

            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);

            SelectedNeighborhood = previously != null
                ? Neighborhoods.FirstOrDefault(n => n.Id == previously.Id)
                : null;
        }

        private void LoadDetails()
        {
            NeighborhoodStreetSections.Clear();
            NeighborhoodMembers.Clear();

            if (SelectedNeighborhood == null) return;

            var sections = _streetSectionService.GetByNeighborhood(SelectedNeighborhood.Id);
            foreach (var s in sections)
                NeighborhoodStreetSections.Add(s);

            var members = _neighborhoodMembershipService.GetCitizensByNeighborhood(SelectedNeighborhood.Id);
            foreach (var m in members)
                NeighborhoodMembers.Add(m);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
