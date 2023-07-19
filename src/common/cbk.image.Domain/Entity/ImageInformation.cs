namespace cbk.image.Domain.Entity
{
    public class ImageInformation
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileLinkPath { get; set; } = string.Empty;
        private ulong _size;
        public ulong Size
        {
            get => _size;
            set
            {
                if (value > 10000000) // Assuming maximum size is 10MB
                {
                    throw new ArgumentException("File size is too large. > 10MB");
                }
                _size = value;
            }
        }
        public bool Status { get; set; }
        public string MediaLink { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
