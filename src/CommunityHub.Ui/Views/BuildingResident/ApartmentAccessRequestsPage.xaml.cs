using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class ApartmentAccessRequestsPage : Page
    {

        private readonly long _userId;
        private readonly ApartmentAccessRequestDbRepository _repository;
        public ObservableCollection<ApartmentAccessRequest> Requests { get; private set; } = new ObservableCollection<ApartmentAccessRequest>();

        public ApartmentAccessRequestsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _repository = new ApartmentAccessRequestDbRepository();

            GetRequests();
        }


        public void GetRequests()
        {
            var requests = _repository.GetByUserId(_userId);
            foreach (var request in requests)
                Requests.Add(request);

            if (Requests.Count > 0)
            {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }
    }
}
