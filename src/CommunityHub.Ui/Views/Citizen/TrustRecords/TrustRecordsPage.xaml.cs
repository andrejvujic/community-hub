using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Windows.Controls;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class TrustRecordsPage : Page
    {
        private readonly long _userId;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _communityEventRepository = new CommunityEventRepository();
        private readonly EventRegistrationRepository _registrationRepository = new EventRegistrationRepository();

        public TrustRecordsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            var citizen = _citizenRepository.GetById(_userId);
            int organized = _communityEventRepository.GetOrganizedEventsCount(_userId);
            int volunteered = _registrationRepository.GetConfirmedVolunteerCount(_userId);
                
            var trustRecord = new CitizenTrustRecord(citizen!, organized, volunteered);
            this.DataContext = trustRecord;
        }

        private void ShowOtherMembersTrustButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new OtherMembersTrustWindow();
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}