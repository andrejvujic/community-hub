using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CitizenMapper : EntityMapper<Citizen>
    {
        private readonly UserMapper _userMapper = new UserMapper();
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private readonly NeighborhoodMembershipMapper _membershipMapper = new NeighborhoodMembershipMapper();

        public override Citizen MapToEntity(IDataReader reader)
        {
            User baseUser = _userMapper.MapToEntity(reader);
            Address address = _addressMapper.MapToEntity(reader);

            int membershipIdOrdinal = reader.GetOrdinal("membership_id");
            NeighborhoodMembership? membership = reader.IsDBNull(membershipIdOrdinal) ? null : _membershipMapper.MapToEntity(reader);

            return new Citizen(
                baseUser.Id,
                baseUser.Username,
                baseUser.Password,
                baseUser.Name,
                baseUser.Surname,
                baseUser.Role,
                address,
                membership
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Citizen entity)
        {
            var parameters = _userMapper.MapToQueryParameters(entity);

            parameters.Add("address_id", entity.Address.Id);
            parameters.Add("membership_id", entity.Membership?.Id);

            return parameters;
        }
    }
}