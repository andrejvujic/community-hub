using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.BuildingManager;
using CommunityHub.Ui.Views.BuildingManager.BuildingPages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class BuildingDetailsPageViewModel : INotifyPropertyChanged
    {
        public User User { get; }
        public Building Building { get; }
        private readonly CommonRoomRentalRequestsPageViewModel _rentalRequestsViewModel;
        private readonly Dictionary<string, Func<Page>> _navigationMap;

        public BuildingDetailsPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            SelectedSection = "Info";
            CurrentPage = new BuildingInfoPage(User, Building);
            _rentalRequestsViewModel = new CommonRoomRentalRequestsPageViewModel(User, Building);
            _rentalRequestsViewModel.AutoProcessRentalRequestStatuses();

            _navigationMap = new Dictionary<string, Func<Page>>
            {
                ["Info"] = () => new BuildingInfoPage(User, Building),
                ["ApartmentAccessRequest"] = () => new ApartmentAccessRequestsPage(User, Building),
                ["ApartmentMembership"] = () => new ApartmentMembershipsPage(User, Building),
                ["CommonRoom"] = () => new CommonRoomsPage(User, Building),
                ["CommonRoomRentalRequest"] = () => new CommonRoomRentalRequestsPage(User, Building),
                ["NoticeBoard"] = () => new NoticeBoardPage(User, Building),
                ["SchedulingAssembly"] = () => new AssemblyPage(User),
                ["ReportedIssue"] = () => new ReportedIssuesPage(User, Building)
            };

            NavigateCommand = new RelayCommand<string>(NavigateTo);
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
            if (tag != null && _navigationMap.TryGetValue(tag, out var createPage))
            {
                SelectedSection = tag;
                CurrentPage = createPage();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}