namespace cbk.image.service.compress.Dto
{
    public class ImageBase
    {

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Cloud Storage Path
        /// </summary>
        public string FileLinkPath { get; set; } = string.Empty;
    }

    public class ImageInformationCompress : ImageBase
    {
       
    }

    public class ImageInformationDto : ImageBase
    {
        /// <summary>
        /// 檔案大小
        /// </summary>
        public ulong? Size { get; set; }
    }
}
