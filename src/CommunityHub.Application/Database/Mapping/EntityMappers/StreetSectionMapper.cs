using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class StreetSectionMapper : EntityMapper<StreetSection>
    {
        private readonly StreetMapper _streetMapper = new StreetMapper();

        public override StreetSection MapToEntity(IDataReader reader)
        {
            Street street = _streetMapper.MapToEntity(reader.WithPrefix("section_street"));

            long id = reader.GetInt64(reader.GetOrdinal("street_section_id"));
            int startNumber = reader.GetInt32(reader.GetOrdinal("start_number"));
            int endNumber = reader.GetInt32(reader.GetOrdinal("end_number"));         
            ParityType side = (ParityType)reader.GetValue(reader.GetOrdinal("side"));

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
