using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class ApartmentAccessRequestsPage : MainNavigatorConsumerPage
    {

        private readonly long _userId;
        private readonly ApartmentAccessRequestRepository _repository;
        public ObservableCollection<ApartmentAccessRequest> Requests { get; private set; } = new ObservableCollection<ApartmentAccessRequest>();

        public ApartmentAccessRequestsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _repository = new ApartmentAccessRequestRepository();

            LoadRequests();
        }


        public void LoadRequests()
        {
            var requests = _repository.GetByApplicantId(_userId);
            foreach (var request in requests)
                Requests.Add(request);

            if (Requests.Count > 0)
            {
                MainContent.Visibility = Visibility.Visible;
                return;
            }

            EmptyContent.Visibility = Visibility.Visible;
        }

        private void OpenRequestDetailsPageButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var request = element.DataContext as ApartmentAccessRequest;
            if (request == null) return;

            Push(new ApartmentAccessRequestDetailsPage(request));
        }
    }
}
