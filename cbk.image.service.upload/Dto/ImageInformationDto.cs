namespace cbk.image.service.upload.Dto
{

    public class ImageBase
    {    
        /// <summary>
        /// Cloud上檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;
    }

    public class ImageDelete : ImageBase
    {

    }

    public class ImageInformationDto : ImageBase
    {
        /// <summary>
        /// 原始檔案名稱
        /// </summary>
        public string OriginalFileName { get; set; } = string.Empty;

        /// <summary>
        /// Cloud Storage Path
        /// </summary>
        public string FileLinkPath { get; set; } = string.Empty;

        /// <summary>
        /// Download Url
        /// </summary>
        public string MediaLink { get; set; } = string.Empty;
        
        /// <summary>
        /// 檔案大小
        /// </summary>
        public ulong? Size { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public ImageInformationDto()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }
}
