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
                id: apartmentAccessRequest.Id,
                applicantId: apartmentAccessRequest.ApplicantId,
                applicant: apartmentAccessRequest.Applicant is not null ? _userMapper.MapToDto(apartmentAccessRequest.Applicant) : null,
                apartmentId: apartmentAccessRequest.ApartmentId,
                apartment: apartmentAccessRequest.Apartment is not null ? _apartmentMapper.MapToDto(apartmentAccessRequest.Apartment) : null,
                comment: apartmentAccessRequest.Comment,
                status: apartmentAccessRequest.Status,
                creationDate: apartmentAccessRequest.CreationDate
            );
        }
        public ApartmentAccessRequest MapToDomain(ApartmentAccessRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ApartmentAccessRequest(
                applicantId: dto.ApplicantId,
                apartmentId: dto.ApartmentId,
                status: dto.Status,
                creationDate: dto.CreationDate,
                id: dto.Id,
                comment: dto.Comment,
                applicant: dto.Applicant is not null ? _userMapper.MapToDomain(dto.Applicant) : null,
                apartment: dto.Apartment is not null ? _apartmentMapper.MapToDomain(dto.Apartment) : null
            );
        }
    }
}