using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ApartmentAccessRequestMapper : EntityMapper<ApartmentAccessRequest>
    {

        private readonly UserMapper _userMapper = new();
        private readonly ApartmentMapper _apartmentMapper = new();

        public override ApartmentAccessRequest MapToEntity(IDataReader reader)
        {
            var applicant = _userMapper.MapToEntity(reader.WithPrefix("applicant"));
            var apartment = _apartmentMapper.MapToEntity(reader);

            if (applicant == null || apartment == null)
                throw new Exception("Couldn't read ApartmentAccessRequest from database because of missing data");

            var id = reader.GetInt64(reader.GetOrdinal("request_id"));
            var status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            
            var commentColumnIndex = reader.GetOrdinal("comment");
            var comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);
            
            var creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));

            return new ApartmentAccessRequest(
                applicant: applicant,
                applicantId: applicant.Id,
                apartment: apartment,
                apartmentId: apartment.Id,
                status: status,
                creationDate: creationDate,
                id: id,
                comment: comment
            ); 
        }

        public override Dictionary<string, object?> MapToQueryParameters(ApartmentAccessRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "applicant_id", entity.ApplicantId },
                { "apartment_id", entity.ApartmentId },
                { "comment", entity.Comment },
                { "status", entity.Status },
                { "creation_date", entity.CreationDate }
            };
        }
    }
}
