using CommunityHub.Application.Database.Repositories;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class TrustRecordsPage : Page
    {
        private readonly long _userId;
        private readonly CommunityEventRepository _communityEventRepository = new CommunityEventRepository();
        private readonly EventRegistrationRepository _registrationRepository = new EventRegistrationRepository();

        public TrustRecordsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            OrganizedEventsCountText.Text = $"Organizovanih događaja: {_communityEventRepository.GetOrganizedEventsCount(_userId)}";
            ConfirmedVolunteerCountText.Text = $"Broj potvrđenih volontiranja: {_registrationRepository.GetConfirmedVolunteerCount(_userId)}";
        }
    }
}