using System.Data;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CoordinatorReviewMapper : EntityMapper<CoordinatorReview>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override CoordinatorReview? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("coordinator_review_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            Neighborhood? neighborhood = _neighborhoodMapper.MapToEntity(reader);
            Citizen? reviewer = _citizenMapper.MapToEntity(reader);
            if (neighborhood is null || reviewer is null) return null;

            int rating = reader.GetInt32(reader.GetOrdinal("rating"));
            string? comment = reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString(reader.GetOrdinal("comment"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("created_at"));
            bool isRemoved = reader.GetBoolean(reader.GetOrdinal("is_removed"));

            return new CoordinatorReview(rating, neighborhood, reviewer, createdAt, comment, isRemoved, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(CoordinatorReview entity)
        {
            return new Dictionary<string, object?>
            {
                { "rating", entity.Rating },
                { "comment", entity.Comment },
                { "created_at", entity.CreatedAt },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "reviewer_id", entity.Reviewer.Id },
                { "is_removed", entity.IsRemoved }
            };
        }
    }
}
