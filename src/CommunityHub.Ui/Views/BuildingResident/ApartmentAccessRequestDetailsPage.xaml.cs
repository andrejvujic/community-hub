using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class ApartmentAccessRequestDetailsPage : MainNavigatorConsumerPage
    {
        public ApartmentAccessRequest Request { get; }

        public ApartmentAccessRequestDetailsPage(ApartmentAccessRequest request)
        {
            InitializeComponent();
            DataContext = this;
            Request = request;
        }
        
        public bool HasComment => Request.Comment != null;
        public bool CanCancelRequest => Request.Status == Status.Waiting;

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
    }
}
