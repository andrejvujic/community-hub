namespace CommunityHub.Application.Domain
{
    public class JobListing
    {
        public long Id { get; private set; }

        public ServiceName ServiceName { get; private set; }

        public string Description { get; private set; }
        public BillingType BillingType { get; private set; }
        public City City { get; private set; }
        public ServiceProvider Provider { get; private set; }
        public List<PriceEntry> PriceList { get; private set; } = new List<PriceEntry>();
        public List<WorkSample> WorkSamples { get; private set; } = new List<WorkSample>();

        public PriceEntry? CurrentPriceEntry =>
            PriceList.FirstOrDefault(e => e.IsActiveOn(DateOnly.FromDateTime(DateTime.Today)));

        public JobListing(
            ServiceName serviceName,
            string description,
            BillingType billingType,
            City city,
            ServiceProvider provider,
            long id = 0,
            List<PriceEntry>? priceList = null,
            List<WorkSample>? workSamples = null
        )
        {
            Id = id;
            ServiceName = serviceName;
            Description = description;
            BillingType = billingType;
            City = city;
            Provider = provider;
            PriceList = priceList ?? new List<PriceEntry>();
            WorkSamples = workSamples ?? new List<WorkSample>();
        }

        public JobListing CopyWith(
            long? id = null,
            ServiceName? serviceName = null,
            string? description = null,
            BillingType? billingType = null,
            City? city = null,
            ServiceProvider? provider = null,
            List<PriceEntry>? priceList = null,
            List<WorkSample>? workSamples = null
        )
        {
            return new JobListing(
                serviceName ?? ServiceName,
                description ?? Description,
                billingType ?? BillingType,
                city ?? City,
                provider ?? Provider,
                id ?? Id,
                priceList ?? PriceList,
                workSamples ?? WorkSamples
            );
        }
    }
}