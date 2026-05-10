using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class NeighborhoodAccessRequestMapper : EntityMapper<NeighborhoodAccessRequest>
    {
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override NeighborhoodAccessRequest MapToEntity(IDataReader reader)
        {
            Citizen citizen = _citizenMapper.MapToEntity(reader);
            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);

            Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
            
            DateTime creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
            
            int commentColumnIndex = reader.GetOrdinal("comment");
            string? comment = reader.IsDBNull(commentColumnIndex) ? null : reader.GetString(commentColumnIndex);

            return new NeighborhoodAccessRequest(citizen, neighborhood, status, creationDate, comment);
        }

        public override Dictionary<string, object?> MapToQueryParameters(NeighborhoodAccessRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "citizen_id", entity.Citizen.Id },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "status", entity.Status },
                { "creation_date", entity.CreationDate },
                { "comment", entity.Comment }
            };
        }
    }
}