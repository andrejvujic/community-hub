namespace CommunityHub.Application.Services.Dtos.Services
{
    public class AvailableJobSlotDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly? Time { get; set; }
        public int? DurationHours { get; set; }
        public decimal Price { get; set; }
        public bool IsPriceChanged { get; set; }

        public AvailableJobSlotDto(
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
