using System.Net.WebSockets;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class StreetSectionRepository
    {
        private readonly StreetSectionMapper _streetSectionMapper = new StreetSectionMapper();
        public int GetStartNumber(long? streetId, long? cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT start_number
                  FROM street_sections INNER JOIN
                  streets ON street_sections.street_id =
                  streets.id INNER JOIN cities ON streets.city_id = cities.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }

        public List<StreetSection> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    section.id AS street_section_id,
                    section.start_number,
                    section.end_number,
                    section.side,
                    street.id AS street_id,
                    street.name AS street_name
                  FROM street_sections section
                  INNER JOIN streets street ON section.street_id = street.id
                  WHERE section.neighborhood_id = @neighborhoodId"
            ).AddParameter("neighborhoodId", neighborhoodId);

            List<StreetSection> sections = new List<StreetSection>();
            query.ExecuteReader(reader => {
                long id = reader.GetInt64(reader.GetOrdinal("street_section_id"));
                int start = reader.GetInt32(reader.GetOrdinal("start_number"));
                int end = reader.GetInt32(reader.GetOrdinal("end_number"));
                ParityType side = (ParityType)reader.GetValue(reader.GetOrdinal("side"));
                long streetId = reader.GetInt64(reader.GetOrdinal("street_id"));
                string streetName = reader.GetString(reader.GetOrdinal("street_name"));

                var street = new Street(streetId, streetName);
                sections.Add(new StreetSection(start, end, side, street, id: id));
            });

            return sections;
        }
        public int GetEndNumber(long? streetId, long? cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT end_number
                  FROM street_sections INNER JOIN
                  streets ON street_sections.street_id =
                  streets.id INNER JOIN cities ON streets.city_id = cities.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }

        public StreetSection Create(StreetSection section, long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO street_sections 
                (start_number, end_number, side, street_id, neighborhood_id) 
                VALUES 
                (@start_number, @end_number, @side, @street_id, @neighborhood_id)
                RETURNING id"
            )
            .ExtendParameters(_streetSectionMapper.MapToQueryParameters(section))
            .AddParameter("neighborhood_id", neighborhoodId);

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return section.CopyWith(id: generatedId);
        }
        public List<StreetSection> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                street_sections.id AS street_section_id, 
                street_sections.start_number, 
                street_sections.end_number, 
                street_sections.side, 
                street_sections.street_id AS section_street_id,
                streets.id AS section_street_street_id,
                streets.name AS section_street_street_name,
                streets.city_id AS section_street_city_id
                FROM street_sections
                INNER JOIN streets ON street_sections.street_id = streets.id
                WHERE street_sections.neighborhood_id = @id"
            ).AddParameter("id", neighborhoodId);

            List<StreetSection> sections = new List<StreetSection>();
            query.ExecuteReader(reader => {
                sections.Add(_streetSectionMapper.MapToEntity(reader));
            });
            return sections;
        }
    }
}
