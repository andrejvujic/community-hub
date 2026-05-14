using System.Collections.ObjectModel;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class RegisterNeighborhoodWindowViewModel
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly CountryService _countryService = new();
        private readonly CityService _cityService = new();
        private readonly StreetService _streetService = new();
        private readonly UserService _userService = new();

        public ObservableCollection<CountryDto> Countries { get; } = new();
        public ObservableCollection<CityDto> Cities { get; } = new();
        public ObservableCollection<StreetDto> Streets { get; } = new();
        public ObservableCollection<StreetSectionDto> AddedStreetSections { get; } = new();
        public ObservableCollection<string> SelectedImagePaths { get; } = new();
        public UserDto? Coordinator { get; private set; }

        public RegisterNeighborhoodWindowViewModel(long coordinatorId)
        {
            Coordinator = _userService.Get(coordinatorId);
        }
    }
}
