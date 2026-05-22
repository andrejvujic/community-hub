using CommunityHub.Application.Domain;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Apartments
{
    public class ApartmentAccessRequestDto
    {
        public long Id { get; set; }
        public long ApplicantId { get; set; }
        public UserDto? Applicant { get; set; }
        public long ApartmentId { get; set; }
        public ApartmentDto? Apartment { get; set; }
        public string? Comment { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }

        public ApartmentAccessRequestDto(
            long applicantId,
            long apartmentId,
            Status status,
            DateTime creationDate,
            string? comment = null,
            UserDto? applicant = null,
            ApartmentDto? apartment = null,
            long id = 0
        )
        {
            Id = id;
            ApplicantId = applicantId;
            Applicant = applicant;
            ApartmentId = apartmentId;
            Apartment = apartment;
            Comment = comment;
            Status = status;
            CreationDate = creationDate;
        }
    }
}