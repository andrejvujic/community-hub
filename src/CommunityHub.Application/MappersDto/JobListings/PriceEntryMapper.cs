using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Application.MappersDto.JobListings
{
    public class PriceEntryMapper : IMapper<PriceEntry, PriceEntryDto>
    {
        public PriceEntryDto MapToDto(PriceEntry priceEntry)
        {
            if (priceEntry == null) throw new ArgumentNullException(nameof(priceEntry));

            return new PriceEntryDto(
                priceEntry.Id,
                priceEntry.Amount,
                priceEntry.ValidFrom,
                priceEntry.ValidTo,
                priceEntry.JobListingId
            );
        }

        public PriceEntry MapToEntity(PriceEntryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new PriceEntry(
                dto.Amount,
                dto.ValidFrom,
                dto.ValidTo,
                dto.JobListingId,
                dto.Id
            );
        }
    }
}
