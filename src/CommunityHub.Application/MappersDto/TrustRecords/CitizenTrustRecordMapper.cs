using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.TrustRecords;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.TrustRecords
{
    public class CitizenTrustRecordMapper : IMapper<CitizenTrustRecord, CitizenTrustRecordDto>
    {
        private readonly CitizenMapper _citizenMapper = new();

        public CitizenTrustRecordDto MapToDto(CitizenTrustRecord citizenTrustRecord)
        {
            if (citizenTrustRecord == null) throw new ArgumentNullException(nameof(citizenTrustRecord));

            return new CitizenTrustRecordDto(
                _citizenMapper.MapToDto(citizenTrustRecord.Citizen),
                citizenTrustRecord.Organized,
                citizenTrustRecord.Volunteered,
                citizenTrustRecord.TrustStatus
            );
        }

        public CitizenTrustRecord MapToEntity(CitizenTrustRecordDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new CitizenTrustRecord(
                _citizenMapper.MapToEntity(dto.Citizen),
                dto.Organized,
                dto.Volunteered
            );
        }
    }
}
