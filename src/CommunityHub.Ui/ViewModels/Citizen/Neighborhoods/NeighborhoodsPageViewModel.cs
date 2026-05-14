using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class NeighborhoodsPageViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodAccessRequestService _neighborhoodAccessRequestService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly CitizenService _citizenService = new();

        private readonly long _userId;
        private CitizenDto? _currentUser;
        private NeighborhoodDto? _selectedNeighborhood;
        private NeighborhoodSearchParameters? _searchParameters;
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CitizenDto? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value) return;
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(Membership));
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
                if (value != null)
                    LoadStreetSections(value);
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value) return;
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public NeighborhoodMembershipDto? Membership => CurrentUser?.Membership;

        public NeighborhoodsPageViewModel(long userId)
        {
            _userId = userId;
            LoadCurrentUser();
            LoadNeighborhoods();
        }

        private void LoadCurrentUser()
        {
            try
            {
                CurrentUser = _citizenService.GetCitizenById(_userId);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Greška pri učitavanju korisnika.";
            }
        }

        public void LoadNeighborhoods()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var neighborhoods = _neighborhoodService.Search(_searchParameters);
                Neighborhoods.Clear();
                foreach (var neighborhood in neighborhoods)
                    Neighborhoods.Add(neighborhood);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Greška pri učitavanju kvartova.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadStreetSections(NeighborhoodDto neighborhood)
        {
            try
            {
                _neighborhoodService.LoadStreetSections(neighborhood);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Greška pri učitavanju ulica.";
            }
        }

        public void PerformSearch(NeighborhoodSearchParameters? searchParameters)
        {
            _searchParameters = searchParameters;
            LoadNeighborhoods();
        }

        public bool RequestNeighborhoodAccess(NeighborhoodDto selectedNeighborhood)
        {
            if (CurrentUser == null)
            {
                ErrorMessage = "Korisnik nije učitan.";
                return false;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Check if already has pending request
                if (_neighborhoodAccessRequestService.HasPendingRequest(_userId, selectedNeighborhood.Id))
                {
                    ErrorMessage = "Već postoji zahtev za pristup ovom kvartu.";
                    return false;
                }

                // Check if citizen's address is in the neighborhood
                if (_neighborhoodService.AddressIsInNeighborhood(selectedNeighborhood, CurrentUser.Address))
                {
                    // Create membership directly
                    _neighborhoodMembershipService.CreateMembershipAndUpdate(CurrentUser, selectedNeighborhood);

                    // Auto-reject other pending requests
                    _neighborhoodAccessRequestService.AutoRejectOtherRequests(CurrentUser, selectedNeighborhood.Id);

                    // Reload current user to get updated membership
                    LoadCurrentUser();

                    return true;
                }
                else
                {
                    // Create access request
                    _neighborhoodAccessRequestService.CreateAccessRequest(CurrentUser, selectedNeighborhood);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Greška pri kreiranju zahteva.";
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
