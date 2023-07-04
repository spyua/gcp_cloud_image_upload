namespace cbk.image.Infrastructure.Database.Entity
{
    public class ImageInformation
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileLinkPath { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
