using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class BuildingMapper : EntityMapper<Building>
    {
        private readonly AddressMapper _addressMapper = new();
        private readonly UserMapper _userMapper = new();
        public override Building MapToEntity(IDataReader reader)
        {
            var manager = _userMapper.MapToEntity(reader.WithPrefix("building_manager"));
            if (manager == null)
                throw new Exception("Building.Manager can't be null");

            var address = _addressMapper.MapToEntity(reader);

            var id = reader.GetInt64(reader.GetOrdinal("building_id"));
            var floorCount = reader.GetInt32(reader.GetOrdinal("floor_count"));
            var apartmentCount = reader.GetInt32(reader.GetOrdinal("apartment_count"));

            return new Building(
                floorCount: floorCount,
                apartmentCount: apartmentCount,
                manager: manager,
                address: address,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Building entity)
        {
            return new Dictionary<string, object?>
            {
                {  "floor_count", entity.FloorCount },
                {  "apartment_count", entity.ApartmentCount },
                {  "address_id", entity.Address.Id },
                {  "building_manager_id", entity.Manager.Id }
            };
        }
    }
}
