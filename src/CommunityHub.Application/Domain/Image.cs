namespace CommunityHub.Application.Domain
{
    public class Image
    {
        public long Id { get; private set; }
        public string Path { get; private set; }
        public Image(long id, string path)
        {
            Id = id;
            Path = path;
        }

        public Image CopyWith(
            long? id = null,
            string? path = null
        )
        {
            return new Image(
                id ?? Id,
                path ?? Path
            );
        }
    }
}
