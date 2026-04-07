using CommunityHub.Application.Domain;
using System.Data;
using System.Security.Principal;
using System.Xml.Linq;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class ApartmentAccessRequestMapper : EntityMapper<ApartmentAccessRequest>
    {

        private readonly UserMapper _userMapper = new UserMapper();
        private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();

        public override ApartmentAccessRequest MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            User applicant = _userMapper.MapToEntity(reader, columnPrefix: "applicant");
            Apartment apartment = _apartmentMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("request_id", columnPrefix));
            Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            
            int commentColumnIndex = reader.GetOrdinalPrefixed("comment", columnPrefix);
            string? comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);
            
            DateTime creationDate = reader.GetDateTime(reader.GetOrdinalPrefixed("creation_date", columnPrefix));

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
