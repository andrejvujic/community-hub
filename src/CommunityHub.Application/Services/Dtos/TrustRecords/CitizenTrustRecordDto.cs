using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.TrustRecords
{
    public class CitizenTrustRecordDto
    {
        public CitizenDto Citizen { get; set; }
        public int Organized { get; set; }
        public int Volunteered { get; set; }
        public TrustStatus TrustStatus { get; set; }

        public CitizenTrustRecordDto(CitizenDto citizen, int organized, int volunteered, TrustStatus trustStatus)
        {
            Citizen = citizen;
            Organized = organized;
            Volunteered = volunteered;
            TrustStatus = trustStatus;
        }
    }
}
