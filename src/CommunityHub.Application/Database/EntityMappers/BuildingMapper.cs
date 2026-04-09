using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class BuildingMapper : EntityMapper<Building>
    {
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private readonly UserMapper _userMapper = new UserMapper();
        public override Building MapToEntity(IDataReader reader)
        {
            Address address = _addressMapper.MapToEntity(reader);
            User manager = _userMapper.MapToEntity(reader.WithPrefix("building_manager"));

            long id = reader.GetInt64(reader.GetOrdinal("building_id"));
            int floorCount = reader.GetInt32(reader.GetOrdinal("floor_count"));

            int? apartmentCount = null;
            if (reader.HasColumn("apartment_count"))
                apartmentCount = reader.GetInt32(reader.GetOrdinal("apartment_count"));

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
