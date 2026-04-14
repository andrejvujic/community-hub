namespace CommunityHub.Application.Domain
{
    public class WorkSample
    {
        public long Id { get; private set; }
        public string Path { get; private set; }
        public long JobListingId { get; private set; }

        public WorkSample(long id, string path, long jobListingId)
        {
            Id = id;
            Path = path;
            JobListingId = jobListingId;
        }

        public WorkSample CopyWith(
            long? id = null,
            string? path = null,
            long? jobListingId = null
        )
        {
            return new WorkSample(
                id ?? Id,
                path ?? Path,
                jobListingId ?? JobListingId
            );
        }
    }
}