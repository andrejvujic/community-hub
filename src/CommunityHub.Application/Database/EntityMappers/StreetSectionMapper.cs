using System.Data;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class StreetSectionMapper : EntityMapper<StreetSection>
    {
        private readonly StreetMapper _streetMapper = new StreetMapper();

        public override StreetSection MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            Street street = _streetMapper.MapToEntity(reader, columnPrefix: "section_street");

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("street_section_id", columnPrefix));
            int startNumber = reader.GetInt32(reader.GetOrdinalPrefixed("start_number", columnPrefix));
            int endNumber = reader.GetInt32(reader.GetOrdinalPrefixed("end_number", columnPrefix));         
            ParityType side = (ParityType)reader.GetValue(reader.GetOrdinalPrefixed("side", columnPrefix));

            return new StreetSection(startNumber, endNumber, side, street, id: id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(StreetSection entity)
        {
            return new Dictionary<string, object?>
            {
                { "start_number", entity.StartNumber },
                { "end_number", entity.EndNumber },
                { "street_id", entity.Street.Id },
                { "side", entity.Side }
            };
        }
    }
}
