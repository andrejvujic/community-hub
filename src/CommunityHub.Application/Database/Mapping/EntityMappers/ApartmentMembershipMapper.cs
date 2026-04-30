using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ApartmentMembershipMapper: EntityMapper<ApartmentMembership>
    {
        private readonly UserMapper _userMapper = new();
        private readonly ApartmentMapper _apartmentMapper = new();

        public override ApartmentMembership MapToEntity(IDataReader reader)
        {
            User? resident = _userMapper.MapToEntity(reader.WithPrefix("resident"));
            Apartment? apartment = _apartmentMapper.MapToEntity(reader);

            if (resident == null || apartment == null)
                throw new Exception("Couldn't read ApartmentMembership from database because of missing data");

            long id = reader.GetInt64(reader.GetOrdinal("membership_id"));

            DateTime acceptanceDate = reader.GetDateTime(reader.GetOrdinal("acceptance_date"));

            return new ApartmentMembership(
                resident,
                apartment,
                acceptanceDate,
                id: id
            );
        }
        public override Dictionary<string, object?> MapToQueryParameters(ApartmentMembership entity)
        {
            return new Dictionary<string, object?>
            {
                { "resident_id", entity.Resident.Id },
                { "apartment_id", entity.Apartment.Id },
                { "acceptance_date", entity.AcceptanceDate }
            };
        }
    }
}
