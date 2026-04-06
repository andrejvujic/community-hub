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

        public override ApartmentAccessRequest ToModel(IDataReader reader)
        {
            User applicant = _userMapper.ToModel(reader);
            Apartment apartment = _apartmentMapper.ToModel(reader);

            long id = reader.GetInt64(reader.GetOrdinal("request_id"));
            Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            string? comment = reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString(reader.GetOrdinal("comment"));
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
