namespace cbk.cloud.serviceProvider.Storage
{
    public class UploadResult
    {
        public string Name { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public ulong Size { get; set; } 
        public string MediaLink { get; set; } = string.Empty;

        public string FileLinkPath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime? TimeCreated { get; set; }
    }
}
