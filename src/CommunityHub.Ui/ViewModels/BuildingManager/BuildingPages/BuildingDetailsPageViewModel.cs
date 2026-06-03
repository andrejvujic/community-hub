using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.Views.BuildingManager;
using CommunityHub.Ui.Views.BuildingManager.BuildingPages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class BuildingDetailsPageViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IAppNavigationService _navigationService;
        private readonly CommonRoomRentalRequestService _commonRoomRentalRequestService;
        private readonly Dictionary<string, Func<Page>> _navigationMap;

        public User User { get; }
        public Building Building { get; }

        public BuildingDetailsPageViewModel(
            User user,
            Building building,
            IAppNavigationService navigationService,
            string? initialSectionTag = null)
        {
            User = user;
            Building = building;
            _navigationService = navigationService;

            _commonRoomRentalRequestService = new();
            _commonRoomRentalRequestService.AutoProcessRentalRequestStatuses(Building.Id);

            _navigationMap = new Dictionary<string, Func<Page>>
            {
                ["Info"] = () => new BuildingInfoPage(User, Building),
                ["ApartmentAccessRequest"] = () => new ApartmentAccessRequestsPage(User, Building),
                ["ApartmentMembership"] = () => new ApartmentMembershipsPage(User, Building),
                ["CommonRoom"] = () => new CommonRoomsPage(User, Building),
                ["CommonRoomRentalRequest"] = () => new CommonRoomRentalRequestsPage(User, Building),
                ["NoticeBoard"] = () => new NoticeBoardPage(User, Building),
                ["SchedulingAssembly"] = () => new SchedulingAssemblyPage(User, Building),
                ["ReportedIssue"] = () => new ReportedIssuesPage(User, Building)
            };

            NavigateCommand = new RelayCommand<string>(NavigateTo);

            _navigationService.Navigated += OnNavigated;

            string firstSection = string.IsNullOrWhiteSpace(initialSectionTag) ? "Info" : initialSectionTag;
            LoadSection(firstSection);
        }

        public ICommand NavigateCommand { get; }

        private string? _selectedSection;
        public string? SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;
                    OnPropertyChanged(nameof(SelectedSection));
                }
            }
        }

        private Page? _currentPage;
        public Page? CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        private void NavigateTo(string? tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return;

            _navigationService.Navigate(new AppNavigationState
            {
                PageTag = "BuildingDetails",
                Building = Building,
                SectionTag = tag
            });
        }

        private void OnNavigated(AppNavigationState state)
        {
            if (state.PageTag != "BuildingDetails")
                return;

            if (state.Building == null)
                return;

            if (state.Building.Id != Building.Id)
                return;

            string section = string.IsNullOrWhiteSpace(state.SectionTag) ? "Info" : state.SectionTag;
            LoadSection(section);
        }

        private void LoadSection(string tag)
        {
            if (_navigationMap.TryGetValue(tag, out var createPage))
            {
                SelectedSection = tag;
                CurrentPage = createPage();
            }
        }
        public void Dispose()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}