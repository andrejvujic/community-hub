using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services;

public class JobListingService
{
    private const decimal MinimumAllowedAverageRating = 2m;

    private readonly JobListingRepository _jobListingRepository;
    private readonly WorkSampleRepository _workSampleRepository;
    private readonly ProviderReviewRepository _providerReviewRepository;

    public JobListingService(
        JobListingRepository jobListingRepository,
        WorkSampleRepository workSampleRepository,
        ProviderReviewRepository providerReviewRepository
    )
    {
        _jobListingRepository = jobListingRepository;
        _workSampleRepository = workSampleRepository;
        _providerReviewRepository = providerReviewRepository;
    }

    public List<JobListing> GetListingsForProvider(long providerId)
    {
        var listings = _jobListingRepository.GetByProviderId(providerId);

        foreach (var listing in listings)
        {
            var workSamples = _workSampleRepository.GetByJobListingId(listing.Id);
            listing.WorkSamples.Clear();
            listing.WorkSamples.AddRange(workSamples);
        }

        return listings;
    }

    public JobListing RegisterListing(
        ServiceProvider provider,
        ServiceName serviceName,
        string description,
        BillingType billingType,
        City city,
        Country country,
        List<PriceEntry> priceEntries,
        List<string> workSamplePaths
    )
    {
        ValidateProviderRating(provider.Id);
        ValidateDescription(description);
        ValidatePriceEntries(priceEntries);
        ValidateWorkSamplePaths(workSamplePaths);

        var listing = new JobListing(
            serviceName,
            description,
            billingType,
            city,
            country,
            provider
        );

        var savedListing = _jobListingRepository.Create(listing);

        var savedPriceEntries = SavePriceEntries(priceEntries, savedListing.Id);
        var savedWorkSamples = SaveWorkSamples(workSamplePaths, savedListing.Id);

        return savedListing.CopyWith(
            priceList: savedPriceEntries,
            workSamples: savedWorkSamples
        );
    }

    private void ValidateProviderRating(long providerId)
    {
        decimal? averageRating = _providerReviewRepository.GetAverageRatingForProvider(providerId);

        if (averageRating.HasValue && averageRating.Value < MinimumAllowedAverageRating)
            throw new InvalidOperationException(
                $"Nije moguće registrovati novu uslugu. Prosečna ocena servisera ({averageRating.Value:F2}) " +
                $"je ispod dozvoljenog minimuma od {MinimumAllowedAverageRating:F0}."
            );
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Opis usluge ne sme biti prazan.", nameof(description));
    }

    private static void ValidatePriceEntries(List<PriceEntry> priceEntries)
    {
        if (priceEntries.Count == 0)
            throw new ArgumentException(
                "Usluga mora imati bar jednu cenu u cenovniku.", nameof(priceEntries));

        foreach (var entry in priceEntries)
        {
            if (entry.Amount <= 0)
                throw new ArgumentException(
                    "Cena mora biti veća od nule.", nameof(priceEntries));

            if (entry.ValidTo < entry.ValidFrom)
                throw new ArgumentException(
                    "Datum kraja važenja cene ne sme biti pre datuma početka važenja.",
                    nameof(priceEntries));
        }
    }

    private static void ValidateWorkSamplePaths(List<string> paths)
    {
        if (paths.Count == 0)
            throw new ArgumentException(
                "Usluga mora imati bar jednu sliku prethodnih radova.", nameof(paths));

        if (paths.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException(
                "Putanja slike ne sme biti prazna.", nameof(paths));
    }

    private List<PriceEntry> SavePriceEntries(List<PriceEntry> priceEntries, long listingId)
    {
        var saved = new List<PriceEntry>(priceEntries.Count);

        foreach (var entry in priceEntries)
        {
            var entryForListing = entry.CopyWith(jobListingId: listingId);
            saved.Add(_jobListingRepository.AddPriceEntry(entryForListing));
        }

        return saved;
    }

    private List<WorkSample> SaveWorkSamples(List<string> paths, long listingId)
    {
        var saved = new List<WorkSample>(paths.Count);

        foreach (var path in paths)
        {
            var sample = new WorkSample(id: 0, path: path, jobListingId: listingId);
            saved.Add(_workSampleRepository.Create(sample));
        }

        return saved;
    }
}