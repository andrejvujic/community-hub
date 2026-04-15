using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class WorkSampleRepository
{
    private readonly WorkSampleMapper _mapper = new WorkSampleMapper();

    public WorkSample Create(WorkSample workSample)
    {
        var query = PostgresQuery.Create(
            @"INSERT INTO work_samples (path, job_listing_id)
              VALUES (@path, @job_listing_id)
              RETURNING id"
        ).ExtendParameters(_mapper.MapToQueryParameters(workSample));

        long newId = query.ExecuteScalar<long>(v => Convert.ToInt64(v));
        return workSample.CopyWith(id: newId);
    }

    public List<WorkSample> GetByJobListingId(long jobListingId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                ws.id AS work_sample_id,
                ws.path AS work_sample_path,
                ws.job_listing_id AS work_sample_listing_id
              FROM work_samples ws
              WHERE ws.job_listing_id = @job_listing_id"
        ).AddParameter("job_listing_id", jobListingId);

        var results = new List<WorkSample>();
        query.ExecuteReader(reader =>
        {
            WorkSample? sample = _mapper.MapToEntity(reader);
            if (sample is not null) results.Add(sample);
        });
        return results;
    }
}