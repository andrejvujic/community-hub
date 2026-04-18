namespace CommunityHub.Application.Domain
{
    public class PriceEntry
    {
        public long Id { get; private set; }
        public decimal Amount { get; private set; }
        public DateOnly ValidFrom { get; private set; }
        public DateOnly ValidTo { get; private set; }
        public long JobListingId { get; private set; }

        public PriceEntry(
            decimal amount,
            DateOnly validFrom,
            DateOnly validTo,
            long jobListingId,
            long id = 0
        )
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    amount,
                    "Cena mora biti veća od nule.");

            if (validTo < validFrom)
                throw new ArgumentException(
                    $"Datum kraja važenja ({validTo}) ne sme biti pre datuma početka ({validFrom}).",
                    nameof(validTo));

            Id = id;
            Amount = amount;
            ValidFrom = validFrom;
            ValidTo = validTo;
            JobListingId = jobListingId;
        }

        public bool IsActiveOn(DateOnly date) => date >= ValidFrom && date <= ValidTo;

        public PriceEntry CopyWith(
            long? id = null,
            decimal? amount = null,
            DateOnly? validFrom = null,
            DateOnly? validTo = null,
            long? jobListingId = null
        )
        {
            return new PriceEntry(
                amount ?? Amount,
                validFrom ?? ValidFrom,
                validTo ?? ValidTo,
                jobListingId ?? JobListingId,
                id ?? Id
            );
        }
    }
}