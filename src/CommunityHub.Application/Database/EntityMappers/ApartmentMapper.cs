using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class ApartmentMapper : EntityMapper<Apartment>
    {
        private readonly BuildingMapper _buildingMapper = new BuildingMapper();
        public override Apartment MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            Building building = _buildingMapper.MapToEntity(reader, columnNamePrefix);

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("apartment_id", columnNamePrefix));
            int number = reader.GetInt32(reader.GetOrdinalPrefixed("apartment_number", columnNamePrefix));
            int floorNumber = reader.GetInt32(reader.GetOrdinalPrefixed("floor_number", columnNamePrefix));

            return new Apartment(id, number, floorNumber, building);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Apartment entity)
        {
            return new Dictionary<string, object?>
            {
                { "apartment_number", entity.Number },
                { "floor_number", entity.FloorNumber }
                // TODO: Add building_id when Building is implemented
            };
        }

    }
}
