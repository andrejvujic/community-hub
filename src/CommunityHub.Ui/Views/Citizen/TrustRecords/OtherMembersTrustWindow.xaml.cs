using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.TrustRecords;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.TrustRecords
{
    public partial class OtherMembersTrustWindow : Window
    {
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _communityEventRepository = new CommunityEventRepository();
        private readonly EventRegistrationRepository _eventRegistrationRepository = new EventRegistrationRepository();

        public OtherMembersTrustWindow(long userId, long neighborhoodId)
        {
            InitializeComponent();

            var citizens = _citizenRepository.GetByNeighborhoodId(neighborhoodId);
            var trustRecords = new List<CitizenTrustRecord>();

            foreach (var citizen in citizens)
            {
                if (citizen.Id == userId) continue;
                int organized = _communityEventRepository.GetOrganizedEventsCount(citizen.Id);
                int volunteered = _eventRegistrationRepository.GetConfirmedVolunteerCount(citizen.Id);
                trustRecords.Add(new CitizenTrustRecord(citizen, organized, volunteered));
            }

            TrustRecordsGrid.ItemsSource = trustRecords;
        }
    }
}
