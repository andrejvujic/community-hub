using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ApartmentMapper : EntityMapper<Apartment>
    {
        private readonly BuildingMapper _buildingMapper = new BuildingMapper();
        public override Apartment? MapToEntity(IDataReader reader)
        {
            int idColumnIndex = reader.GetOrdinal("apartment_id");

            if (reader.IsDBNull(idColumnIndex)) return null;
            long id = reader.GetInt64(idColumnIndex);

            Building building = _buildingMapper.MapToEntity(reader);

            int number = reader.GetInt32(reader.GetOrdinal("apartment_number"));
            int floorNumber = reader.GetInt32(reader.GetOrdinal("floor_number"));

            return new Apartment(id, number, floorNumber, building);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Apartment entity)
        {
            return new Dictionary<string, object?>
            {
                { "apartment_number", entity.Number },
                { "floor_number", entity.FloorNumber },
                { "building_id", entity.Building.Id }
            };
        }

    }
}
