using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class ApartmentMapper : EntityMapper<Apartment>
    {
        public override Apartment MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("apartment_id", columnPrefix));
            int number = reader.GetInt32(reader.GetOrdinalPrefixed("apartment_number", columnPrefix));
            int floorNumber = reader.GetInt32(reader.GetOrdinalPrefixed("floor_number", columnPrefix));
            return new Apartment(id, number, floorNumber);
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
