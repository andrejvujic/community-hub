namespace CommunityHub.Application.Services.Dtos.Services
{
    public class PriceEntryDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly ValidTo { get; set; }
        public long JobListingId { get; set; }

        public bool IsActiveOn(DateOnly date) => date >= ValidFrom && date <= ValidTo;

        public PriceEntryDto(long id, decimal amount, DateOnly validFrom, DateOnly validTo, long jobListingId)
        {
            Id = id;
            Amount = amount;
            ValidFrom = validFrom;
            ValidTo = validTo;
            JobListingId = jobListingId;
        }
    }
}
