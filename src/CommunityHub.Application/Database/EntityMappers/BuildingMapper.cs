using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class BuildingMapper : EntityMapper<Building>
    {
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private readonly UserMapper _userMapper = new UserMapper();
        public override Building MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            Address address = _addressMapper.MapToEntity(reader);
            User manager = _userMapper.MapToEntity(
                reader,
                columnNamePrefix: columnNamePrefix == null ? "building_manager" : $"{columnNamePrefix}_building_manager"
            );

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("address_id", columnNamePrefix));
            int floorCount = reader.GetInt32(reader.GetOrdinalPrefixed("floor_count", columnNamePrefix));
            int apartmentCount = reader.GetInt32(reader.GetOrdinalPrefixed("apartment_count", columnNamePrefix));

            return new Building(floorCount, apartmentCount, manager, address, id: id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Building entity)
        {
            return new Dictionary<string, object?>
            {
                {  "floor_count", entity.FloorCount },
                {  "address_id", entity.Address.Id },
                {  "building_manager_id", entity.Manager.Id }
            };
        }
    }
}
