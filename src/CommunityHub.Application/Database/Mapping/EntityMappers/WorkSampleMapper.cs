using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class WorkSampleMapper : EntityMapper<WorkSample>
    {
        public override WorkSample? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("work_sample_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            string path = reader.GetString(reader.GetOrdinal("work_sample_path"));
            long jobListingId = reader.GetInt64(reader.GetOrdinal("work_sample_listing_id"));

            return new WorkSample(id, path, jobListingId);
        }

        public override Dictionary<string, object?> MapToQueryParameters(WorkSample entity)
        {
            return new Dictionary<string, object?>
            {
                { "path", entity.Path },
                { "job_listing_id", entity.JobListingId }
            };
        }
    }
}