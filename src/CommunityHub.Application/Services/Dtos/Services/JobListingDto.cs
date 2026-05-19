using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Images;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Services
{
    public class JobListingDto
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public BillingType BillingType { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }
        public ServiceProviderDto Provider { get; set; }
        public List<PriceEntryDto> PriceList { get; set; }
        public List<ImageDto> WorkSamples { get; set; }

        public PriceEntryDto? CurrentPriceEntry =>
            PriceList.FirstOrDefault(e => e.IsActiveOn(DateOnly.FromDateTime(DateTime.Today)));

        public JobListingDto(
            long id,
            string serviceName,
            string description,
            BillingType billingType,
            CityDto city,
            CountryDto country,
            ServiceProviderDto provider,
            List<PriceEntryDto>? priceList = null,
            List<ImageDto>? workSamples = null)
        {
            Id = id;
            ServiceName = serviceName;
            Description = description;
            BillingType = billingType;
            City = city;
            Country = country;
            Provider = provider;
            PriceList = priceList ?? new();
            WorkSamples = workSamples ?? new();
        }
    }
}
