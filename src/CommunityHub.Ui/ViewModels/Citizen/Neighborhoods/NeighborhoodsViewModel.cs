using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Images;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
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

        private int _currentImagePage = 0;
        private const int IMAGES_PER_PAGE = 3;

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

        public ObservableCollection<ImageDto> CurrentImages { get; } = new();

        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                _currentImagePage = 0;
                OnPropertyChanged(nameof(SelectedNeighborhood));
                OnPropertyChanged(nameof(CanRequestAccess));
                OnPropertyChanged(nameof(CurrentImages));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(CurrentPageDisplay));
                UpdateCurrentImages();
                if (value != null) _neighborhoodService.LoadStreetSections(value);
            }
        }

        public NeighborhoodMembershipDto? Membership => CurrentUser?.Membership;

        public bool CanRequestAccess => SelectedNeighborhood != null && !_neighborhoodAccessRequestService.HasPendingRequest(_userId, SelectedNeighborhood.Id) && Membership == null;

        public NeighborhoodSearchParameters? LastSearchParameters => _searchParameters;

        public ICommand SearchCommand { get; }
        public ICommand AccessOrCoordinatorCommand { get; }
        public ICommand MyAccessRequestsCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand NextImagePageCommand { get; }
        public ICommand PreviousImagePageCommand { get; }

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
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());
            NextImagePageCommand = new RelayCommand<object>(_ => NextImagePage(), _ => CanGoNext);
            PreviousImagePageCommand = new RelayCommand<object>(_ => PreviousImagePage(), _ => CanGoPrevious);

            CurrentUser = _citizenService.GetCitizenById(_userId);
            LoadNeighborhoods();
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Kvartovi",
                "Pretražujte kvartove i pregledajte detalje o izabranim kvartovima. Ako niste član kvarta, možete podneti zahtev za pristup. " +
                "U suprotnom, možete oceniti svojeg koordinatora, pregledati recenzije i prijaviti ih. Možete i pregledati sve poslate zahteve.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+S", Description = "Napredna pretraga kvartova" },
                    new() { Keys = "Ctrl+1", Description = "Pošalji zahtev za pristup kvartu (bez članstva)" },
                    new() { Keys = "Ctrl+1", Description = "Prikaži koordinatora kvarta (sa članstvom)" },
                    new() { Keys = "Ctrl+2", Description = "Prikaži svoje zahteve za pristup" },
                }
            );
            helpWindow.ShowDialog();
        }

        public void LoadNeighborhoods()
        {
            var neighborhoods = _neighborhoodService.Search(_searchParameters);
            var selectedId = _selectedNeighborhood?.Id;

            Neighborhoods.Clear();
            foreach (var neighborhood in neighborhoods) Neighborhoods.Add(neighborhood);

            SelectedNeighborhood = Neighborhoods.FirstOrDefault(n => n.Id == selectedId) ?? Neighborhoods.FirstOrDefault();
            UpdateCurrentImages();
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

        public int TotalPages => SelectedNeighborhood?.Images?.Count > 0 ? (int)Math.Ceiling((double)SelectedNeighborhood.Images.Count / IMAGES_PER_PAGE) : 0;
        public string CurrentPageDisplay => TotalPages > 0 ? $"{CurrentImagePage + 1}/{TotalPages}": "Nema slika";
        public bool CanGoPrevious => CurrentImagePage > 0;
        public bool CanGoNext => CurrentImagePage < TotalPages - 1;

        public int CurrentImagePage
        {
            get => _currentImagePage;
            set
            {
                if (_currentImagePage == value) return;
                _currentImagePage = Math.Clamp(value, 0, TotalPages - 1);
                OnPropertyChanged(nameof(CurrentImagePage));
                OnPropertyChanged(nameof(CurrentImages));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(CurrentPageDisplay));
            }
        }

        private void UpdateCurrentImages()
        {
            CurrentImages.Clear();
            if (SelectedNeighborhood?.Images == null || SelectedNeighborhood.Images.Count == 0) return;

            var startIndex = CurrentImagePage * IMAGES_PER_PAGE;
            var pageImages = SelectedNeighborhood.Images.Skip(startIndex).Take(IMAGES_PER_PAGE).ToList();

            foreach (var image in pageImages) CurrentImages.Add(image);
        }

        private void NextImagePage()
        {
            if (CanGoNext)
            {
                CurrentImagePage++;
                UpdateCurrentImages();
            }
        }

        private void PreviousImagePage()
        {
            if (CanGoPrevious)
            {
                CurrentImagePage--;
                UpdateCurrentImages();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
