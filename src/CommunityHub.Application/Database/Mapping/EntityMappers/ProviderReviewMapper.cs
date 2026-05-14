using CommunityHub.Application.Domain.ServiceBookings;
using CommunityHub.Application.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ProviderReviewMapper : EntityMapper<ProviderReview>
    {
        private readonly BookedJobMapper _bookedJobMapper = new BookedJobMapper();
        private readonly UserMapper _userMapper = new UserMapper();

        public override ProviderReview? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("review_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            BookedJob? bookedJob = _bookedJobMapper.MapToEntity(reader);
            if (bookedJob is null) return null;

            User? coordinator = _userMapper.MapToEntity(reader.WithPrefix("coordinator"));
            if (coordinator is null) return null;

            int qualityRating = reader.GetInt32(reader.GetOrdinal("quality_rating"));
            int communicationRating = reader.GetInt32(reader.GetOrdinal("communication_rating"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("review_created_at"));

            int commentOrdinal = reader.GetOrdinal("review_comment");
            string? comment = reader.IsDBNull(commentOrdinal)
                ? null
                : reader.GetString(commentOrdinal);

            int updatedAtOrdinal = reader.GetOrdinal("updated_at");
            DateTime? updatedAt = reader.IsDBNull(updatedAtOrdinal)
                ? null
                : reader.GetDateTime(updatedAtOrdinal);

            return new ProviderReview(
                bookedJob,
                coordinator,
                qualityRating,
                communicationRating,
                createdAt,
                id: id,
                comment: comment,
                updatedAt: updatedAt
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(ProviderReview entity)
        {
            return new Dictionary<string, object?>
            {
                { "quality_rating", entity.QualityRating },
                { "communication_rating", entity.CommunicationRating },
                { "comment", entity.Comment },
                { "updated_at", entity.UpdatedAt },
                { "booked_job_id", entity.BookedJob.Id },
                { "coordinator_id", entity.Coordinator.Id }
            };
        }
    }
}