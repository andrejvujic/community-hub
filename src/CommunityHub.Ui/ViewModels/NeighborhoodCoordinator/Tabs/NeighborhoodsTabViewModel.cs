using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class NeighborhoodsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly StreetSectionService _streetSectionService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<StreetSectionDto> NeighborhoodStreetSections { get; } = new();
        public ObservableCollection<CitizenDto> NeighborhoodMembers { get; } = new();

        public RelayCommand RegisterCommand { get; }

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
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        private void ExecuteRegister()
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                         ?? System.Windows.Application.Current.MainWindow;

            var dialog = new RegisterNeighborhoodWindow(_coordinatorId)
            {
                Owner = window
            };

            if (dialog.ShowDialog() == true)
            {
                LoadNeighborhoods();
            }
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