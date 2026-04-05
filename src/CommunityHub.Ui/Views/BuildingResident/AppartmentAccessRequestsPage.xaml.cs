using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class AppartmentAccessRequestsPage : Page
    {

        private readonly long _userId;
        private readonly AppartmentAccessRequestDbRepository _repository;
        public ObservableCollection<AppartmentAccessRequest> Requests { get; private set; } = new ObservableCollection<AppartmentAccessRequest>();

        public AppartmentAccessRequestsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _repository = new AppartmentAccessRequestDbRepository();

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
