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

        public override ApartmentAccessRequest MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            User applicant = _userMapper.MapToEntity(
                reader,
                columnNamePrefix: columnNamePrefix == null ? "applicant" : $"{columnNamePrefix}_applicant"
            );
            Apartment apartment = _apartmentMapper.MapToEntity(reader, columnNamePrefix);

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("request_id", columnNamePrefix));
            Status status = (Status)reader.GetValue(reader.GetOrdinalPrefixed("status", columnNamePrefix));
            
            int commentColumnIndex = reader.GetOrdinalPrefixed("comment", columnNamePrefix);
            string? comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);
            
            DateTime creationDate = reader.GetDateTime(reader.GetOrdinalPrefixed("creation_date", columnNamePrefix));

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
