namespace CommunityHub.Application.Domain.JobListings
{
    public class AvailableJobSlot
    {
        public DateOnly Date { get; }
        public TimeOnly? Time { get; }
        public int? DurationHours { get; }
        public decimal Price { get; }
        public bool IsPriceChanged { get; }

        public AvailableJobSlot(
            DateOnly date,
            decimal price,
            bool isPriceChanged,
            TimeOnly? time = null,
            int? durationHours = null)
        {
            Date = date;
            Price = price;
            IsPriceChanged = isPriceChanged;
            Time = time;
            DurationHours = durationHours;
        }
    }
}
