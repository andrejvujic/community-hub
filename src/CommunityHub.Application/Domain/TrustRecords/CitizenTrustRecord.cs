using CommunityHub.Application.Domain.Users;
using System;

namespace CommunityHub.Application.Domain.TrustRecords
{
    public class CitizenTrustRecord
    {
        public Citizen Citizen { get; }
        public int Organized { get; }
        public int Volunteered { get; }
        public TrustStatus TrustStatus => CalculateTrustStatus();

        public CitizenTrustRecord(Citizen citizen, int organized, int volunteered)
        {
            Citizen = citizen;
            Organized = organized;
            Volunteered = volunteered;
        }

        private TrustStatus CalculateTrustStatus()
        {
            var totalEvents = Organized + Volunteered;
            var membershipDuration = Citizen?.Membership != null ? DateTime.Now - Citizen.Membership.AcceptanceDate : TimeSpan.MaxValue;

            if (membershipDuration.TotalDays < 365)
                return TrustStatus.New;
            if (totalEvents < 5)
                return TrustStatus.Inactive;
            if (totalEvents >= 5 && totalEvents < 10)
                return TrustStatus.Active;
            if (totalEvents >= 10 && totalEvents <= 15)
                return TrustStatus.Prominent;
            if (totalEvents > 15)
                return TrustStatus.Trustworthy;
            return TrustStatus.Inactive;
        }
    }
}
