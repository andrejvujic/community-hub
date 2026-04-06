using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public class ApartmentAccessRequestMapper : Mapper<ApartmentAccessRequest>
    {
        private ApartmentAccessRequestMapper() { }
        public static ApartmentAccessRequestMapper Instance => new ApartmentAccessRequestMapper();

        private readonly UserMapper _userMapper = UserMapper.Instance;
        private readonly ApartmentMapper _apartmentMapper = ApartmentMapper.Instance;

        public override ApartmentAccessRequest ConvertToEntity(IDataReader reader)
        {
            User applicant = _userMapper.ConvertToEntity(reader);
            Apartment apartment = _apartmentMapper.ConvertToEntity(reader);

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
    }
}
