namespace cbk.cloud.serviceProvider.Storage
{
    public class UploadResult
    {
        public string Name { get; set; }
        public string Bucket { get; set; }
        public ulong? Size { get; set; }
        public string MediaLink { get; set; }

        public string FileLinkPath { get; set; }
        public string ContentType { get; set; }
        public DateTime? TimeCreated { get; set; }
    }
}
