using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.MappersDto.Images;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Application.MappersDto.JobListings
{
    public class JobListingMapper : IMapper<JobListing, JobListingDto>
    {
        private readonly CityMapper _cityMapper = new();
        private readonly CountryMapper _countryMapper = new();
        private readonly ServiceProviderMapper _providerMapper = new();
        private readonly PriceEntryMapper _priceEntryMapper = new();
        private readonly ImageMapper _imageMapper = new();

        public JobListingDto MapToDto(JobListing jobListing)
        {
            if (jobListing == null) throw new ArgumentNullException(nameof(jobListing));

            return new JobListingDto(
                jobListing.Id,
                jobListing.ServiceName,
                jobListing.Description,
                jobListing.BillingType,
                _cityMapper.MapToDto(jobListing.City),
                _countryMapper.MapToDto(jobListing.Country),
                _providerMapper.MapToDto(jobListing.Provider),
                jobListing.PriceList.Select(_priceEntryMapper.MapToDto).ToList(),
                jobListing.WorkSamples.Select(_imageMapper.MapToDto).ToList());
        }

        public JobListing MapToEntity(JobListingDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new JobListing(
                dto.ServiceName,
                dto.Description,
                dto.BillingType,
                _cityMapper.MapToEntity(dto.City),
                _countryMapper.MapToEntity(dto.Country),
                _providerMapper.MapToEntity(dto.Provider),
                dto.Id,
                dto.PriceList.Select(_priceEntryMapper.MapToEntity),
                dto.WorkSamples.Select(_imageMapper.MapToEntity));
        }
    }
}