using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.Apartments
{
    public class ApartmentAccessRequestMapper : IMapper<ApartmentAccessRequest, ApartmentAccessRequestDto>
    {
        private readonly UserMapper _userMapper = new();
        private readonly ApartmentMapper _apartmentMapper = new();
        public ApartmentAccessRequestDto MapToDto(ApartmentAccessRequest apartmentAccessRequest)
        {
            if (apartmentAccessRequest == null) throw new ArgumentNullException(nameof(apartmentAccessRequest));

            return new ApartmentAccessRequestDto(
                apartmentAccessRequest.Id,
                apartmentAccessRequest.ApplicantId,
                apartmentAccessRequest.Applicant != null ? _userMapper.MapToDto(apartmentAccessRequest.Applicant) : null,
                apartmentAccessRequest.ApartmentId,
                apartmentAccessRequest.Apartment != null ? _apartmentMapper.MapToDto(apartmentAccessRequest.Apartment) : null,
                apartmentAccessRequest.Comment,
                apartmentAccessRequest.Status,
                apartmentAccessRequest.CreationDate
            );
        }
        public ApartmentAccessRequest MapToDomain(ApartmentAccessRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ApartmentAccessRequest(
                dto.ApplicantId,
                dto.ApartmentId,
                dto.Status,
                dto.CreationDate,
                dto.Id,
                dto.Comment,
                dto.Applicant != null ? _userMapper.MapToDomain(dto.Applicant) : null,
                apartment: dto.Apartment != null ? _apartmentMapper.MapToDomain(dto.Apartment) : null
            );
        }
    }
}