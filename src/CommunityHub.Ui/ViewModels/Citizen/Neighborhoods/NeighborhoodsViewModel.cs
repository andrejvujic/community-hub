using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class NeighborhoodsViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodAccessRequestService _neighborhoodAccessRequestService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly CitizenService _citizenService = new();

        private readonly long _userId;
        private CitizenDto? _currentUser;
        private NeighborhoodDto? _selectedNeighborhood;
        private NeighborhoodSearchParameters? _searchParameters;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? SearchRequested;
        public event EventHandler? RequestAccessRequested;
        public event EventHandler? CoordinatorRequested;
        public event EventHandler? MyAccessRequestsRequested;

        public CitizenDto? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value) return;
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(Membership));
                OnPropertyChanged(nameof(CanRequestAccess));
            }
        }

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();

        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged(nameof(SelectedNeighborhood));
                OnPropertyChanged(nameof(CanRequestAccess));
                if (value != null) _neighborhoodService.LoadStreetSections(value);
            }
        }

        public NeighborhoodMembershipDto? Membership => CurrentUser?.Membership;

        public bool CanRequestAccess => SelectedNeighborhood != null && !_neighborhoodAccessRequestService.HasPendingRequest(_userId, SelectedNeighborhood.Id) && Membership == null;

        public NeighborhoodSearchParameters? LastSearchParameters => _searchParameters;

        public ICommand SearchCommand { get; }
        public ICommand AccessOrCoordinatorCommand { get; }
        public ICommand MyAccessRequestsCommand { get; }

        public NeighborhoodsViewModel(long userId)
        {
            _userId = userId;

            SearchCommand = new RelayCommand<object>(_ => SearchRequested?.Invoke(this, EventArgs.Empty));
            AccessOrCoordinatorCommand = new RelayCommand<object>(_ =>
            {
                if (Membership != null) CoordinatorRequested?.Invoke(this, EventArgs.Empty);
                else if (CanRequestAccess) RequestAccessRequested?.Invoke(this, EventArgs.Empty);
            });
            MyAccessRequestsCommand = new RelayCommand<object>(_ => MyAccessRequestsRequested?.Invoke(this, EventArgs.Empty));

            CurrentUser = _citizenService.GetCitizenById(_userId);
            LoadNeighborhoods();
        }

        public void LoadNeighborhoods()
        {
            var neighborhoods = _neighborhoodService.Search(_searchParameters);
            var selectedId = _selectedNeighborhood?.Id;

            Neighborhoods.Clear();
            foreach (var neighborhood in neighborhoods) Neighborhoods.Add(neighborhood);

            SelectedNeighborhood = Neighborhoods.FirstOrDefault(n => n.Id == selectedId) ?? Neighborhoods.FirstOrDefault();
        }

        public void PerformSearch(NeighborhoodSearchParameters? searchParameters)
        {
            _searchParameters = searchParameters;
            LoadNeighborhoods();
        }

        public bool RequestNeighborhoodAccess(NeighborhoodDto selectedNeighborhood)
        {
            if (CurrentUser == null)
                return false;

            if (_neighborhoodService.AddressIsInNeighborhood(selectedNeighborhood, CurrentUser.Address))
            {
                _neighborhoodMembershipService.CreateMembershipAndUpdate(CurrentUser, selectedNeighborhood);
                _neighborhoodAccessRequestService.AutoRejectOtherRequests(CurrentUser, selectedNeighborhood.Id);
                CurrentUser = _citizenService.GetCitizenById(_userId);
            }
            else
            {
                _neighborhoodAccessRequestService.CreateAccessRequest(CurrentUser, selectedNeighborhood);
            }

            return true;
        }

        public string GetAccessResultMessage(NeighborhoodDto neighborhood)
        {
            if (CurrentUser?.Membership != null)
            {
                return "Vaša adresa pripada ovom kvartu. Bićete automatski pridruženi kvartu.";
            }
            else
            {
                return "Vaša adresa ne pripada ovom kvartu. Biće kreiran zahtev za pristup.";
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
