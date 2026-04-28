using System;

namespace CommunityHub.Application.Domain
{
    public class CitizenTrustRecord
    {
        public string Name { get; }
        public string Surname { get; } 
        public int Organized { get; }
        public int Volunteered { get; }
        public DateTime JoiningDate { get; }
        public TrustStatus TrustStatus => CalculateTrustStatus();

        public CitizenTrustRecord(string name, string surname, int organized, int volunteered, DateTime joiningDate)
        {
            Name = name;
            Surname = surname;
            Organized = organized;
            Volunteered = volunteered;
            JoiningDate = joiningDate;
        }

        private TrustStatus CalculateTrustStatus()
        {
            var totalEvents = Organized + Volunteered;
            var membershipDuration = DateTime.Now - JoiningDate;

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
