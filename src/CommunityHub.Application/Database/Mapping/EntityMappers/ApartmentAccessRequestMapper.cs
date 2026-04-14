using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ApartmentAccessRequestMapper : EntityMapper<ApartmentAccessRequest>
    {

        private readonly UserMapper _userMapper = new();
        private readonly ApartmentMapper _apartmentMapper = new();

        public override ApartmentAccessRequest MapToEntity(IDataReader reader)
        {
            User? applicant = _userMapper.MapToEntity(reader.WithPrefix("applicant"));
            Apartment? apartment = _apartmentMapper.MapToEntity(reader);

            if (applicant == null || apartment == null)
                throw new Exception("Couldn't read ApartmentAccessRequest from database because of missing data");

            long id = reader.GetInt64(reader.GetOrdinal("request_id"));
            Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            
            int commentColumnIndex = reader.GetOrdinal("comment");
            string? comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);
            
            DateTime creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));

            return new ApartmentAccessRequest(
                applicant,
                apartment,
                status,
                creationDate,
                id: id,
                comment: comment
            ); 
        }

        public override Dictionary<string, object?> MapToQueryParameters(ApartmentAccessRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "applicant_id", entity.Applicant.Id },
                { "apartment_id", entity.Apartment.Id },
                { "comment", entity.Comment },
                { "status", entity.Status },
                { "creation_date", entity.CreationDate }
            };
        }
    }
}
