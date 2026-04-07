using CommunityHub.Application.Domain;
using System.Data;
using System.Security.Principal;
using System.Xml.Linq;

namespace CommunityHub.Application.Database.Mappers
{
    public class ApartmentAccessRequestMapper : DatabaseEntityMapper<ApartmentAccessRequest>
    {

        private readonly UserMapper _userMapper = new UserMapper();
        private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();

        public override ApartmentAccessRequest MapToEntity(IDataReader reader)
        {
            User applicant = _userMapper.MapToEntity(reader);
            Apartment apartment = _apartmentMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinal("request_id"));
            Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            
            int commentColumnIndex = reader.GetOrdinal("comment");
            string? comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);
            
            DateTime creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));

            return new ApartmentAccessRequest(
                id,
                applicant,
                apartment,
                comment,
                status,
                creationDate
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
