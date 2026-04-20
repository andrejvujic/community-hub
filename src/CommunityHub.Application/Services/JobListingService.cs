using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services;

public class JobListingService
{
    private const decimal MinimumAllowedAverageRating = 2m;

    private readonly JobListingRepository _jobListingRepository;
    private readonly ProviderReviewRepository _reviewRepository;
    private readonly ImageRepository _imageRepository;

    public JobListingService(
        JobListingRepository jobListingRepository,
        ProviderReviewRepository reviewRepository,
        ImageRepository imageRepository)
    {
        _jobListingRepository = jobListingRepository;
        _reviewRepository = reviewRepository;
        _imageRepository = imageRepository;
    }

    // Vraća sve usluge koje serviser pruža
    public List<JobListing> GetListingsForProvider(long providerId)
        => _jobListingRepository.GetByProviderId(providerId);

    // Registruje novu uslugu. Serviser čija prosečna ocena je ispod 2 ne može registrovati nove usluge
    public JobListing RegisterListing(
        ServiceProvider provider,
        string serviceName,
        string description,
        BillingType billingType,
        City city,
        Country country,
        List<PriceEntry> priceEntries,
        List<string> workSamplePaths)
    {
        ValidateProviderRating(provider.Id);
        ValidatePriceEntries(priceEntries);
        ValidateWorkSamplePaths(workSamplePaths);

        var workSamples = workSamplePaths
            .Select(path => Image.ForJobListing(id: 0, path: path, jobListingId: 0))
            .ToList();

        var listing = new JobListing(
            serviceName,
            description,
            billingType,
            city,
            country,
            provider,
            priceList: priceEntries,
            workSamples: workSamples
        );

        return _jobListingRepository.Create(listing);
    }

    // Dodaje novu cenu u cenovnik postojeće usluge
    public PriceEntry AddPriceEntry(long jobListingId, PriceEntry entry)
    {
        var entryForListing = entry.CopyWith(jobListingId: jobListingId);
        return _jobListingRepository.AddPriceEntry(entryForListing);
    }

    // Ažurira slike prethodnih radova za postojeću uslugu; briše sve stare slike i upisuje nove putanje.
    public List<Image> UpdateWorkSamples(long jobListingId, List<string> newPaths)
    {
        ValidateWorkSamplePaths(newPaths);

        _imageRepository.DeleteByJobListingId(jobListingId);

        var saved = new List<Image>();
        foreach (var path in newPaths)
        {
            var placeholder = Image.ForJobListing(id: 0, path: path, jobListingId: jobListingId);
            var created = _imageRepository.Create(placeholder);
            saved.Add(created);
        }
        return saved;
    }

    private void ValidateProviderRating(long providerId)
    {
        decimal? avg = _reviewRepository.GetAverageRatingForProvider(providerId);

        // Ako serviser nema nijednu recenziju, prosečna ocena ne postoji, ograničenje se ne primenjuje...
        if (avg is null) return;

        if (avg < MinimumAllowedAverageRating)
            throw new InvalidOperationException(
                $"Serviser ne može registrovati nove usluge dok mu je prosečna ocena " +
                $"({avg:F2}) ispod {MinimumAllowedAverageRating}.");
    }

    private static void ValidatePriceEntries(List<PriceEntry> priceEntries)
    {
        if (priceEntries.Count == 0)
            throw new ArgumentException(
                "Usluga mora imati bar jednu cenu u cenovniku.", nameof(priceEntries));
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
}