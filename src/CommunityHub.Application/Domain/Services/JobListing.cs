using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Services
{
    public class JobListing
    {
        public long Id { get; private set; }
        public string ServiceName { get; private set; }
        public string Description { get; private set; }
        public BillingType BillingType { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }
        public ServiceProvider Provider { get; private set; }

        private readonly List<PriceEntry> _priceList = new List<PriceEntry>();
        private readonly List<Image> _workSamples = new List<Image>();

        public IReadOnlyList<PriceEntry> PriceList => _priceList.AsReadOnly();
        public IReadOnlyList<Image> WorkSamples => _workSamples.AsReadOnly();

        public PriceEntry? CurrentPriceEntry =>
            _priceList.FirstOrDefault(e => e.IsActiveOn(DateOnly.FromDateTime(DateTime.Today)));

        public JobListing(
            string serviceName,
            string description,
            BillingType billingType,
            City city,
            Country country,
            ServiceProvider provider,
            long id = 0,
            IEnumerable<PriceEntry>? priceList = null,
            IEnumerable<Image>? workSamples = null
        )
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentException("Naziv usluge ne sme biti prazan.", nameof(serviceName));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Opis usluge ne sme biti prazan.", nameof(description));

            Id = id;
            ServiceName = serviceName;
            Description = description;
            BillingType = billingType;
            City = city;
            Country = country;
            Provider = provider;

            if (priceList is not null) _priceList.AddRange(priceList);
            if (workSamples is not null) _workSamples.AddRange(workSamples);
        }

        public void AddPriceEntry(PriceEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            _priceList.Add(entry);
        }

        public void AddWorkSample(Image image)
        {
            ArgumentNullException.ThrowIfNull(image);
            _workSamples.Add(image);
        }

        public void SetWorkSamples(IEnumerable<Image> images)
        {
            ArgumentNullException.ThrowIfNull(images);
            _workSamples.Clear();
            _workSamples.AddRange(images);
        }

        public JobListing CopyWith(
            long? id = null,
            string? serviceName = null,
            string? description = null,
            BillingType? billingType = null,
            City? city = null,
            Country? country = null,
            ServiceProvider? provider = null,
            IEnumerable<PriceEntry>? priceList = null,
            IEnumerable<Image>? workSamples = null
        )
        {
            return new JobListing(
                serviceName ?? ServiceName,
                description ?? Description,
                billingType ?? BillingType,
                city ?? City,
                country ?? Country,
                provider ?? Provider,
                id ?? Id,
                priceList ?? _priceList,
                workSamples ?? _workSamples
            );
        }
    }
}