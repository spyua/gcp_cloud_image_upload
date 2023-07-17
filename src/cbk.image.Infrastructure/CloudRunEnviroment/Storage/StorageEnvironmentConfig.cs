namespace cbk.image.Infrastructure.CloudRunEnviroment.Storage
{
    public class StorageEnvironmentConfig
    {
        public string ImageBucket { get; private set; }
        public string OriginalImageBucket { get; private set; }

        public StorageEnvironmentConfig(bool useMock = false)
        {
            if (useMock)
            {
                // Set Mock Data
                ImageBucket = "cbk_mario_test_project_image_bucket";
                OriginalImageBucket = "cbk_mario_test_project_original_image_bucket";
            }
            else
            {
                // Get environment variable or set to default
                ImageBucket = Environment.GetEnvironmentVariable("IMAGE_BUCKET") ?? "";
                OriginalImageBucket = Environment.GetEnvironmentVariable("ORIGINAL_IMAGE_BUCKET") ?? "";
            }
        }

    }
}
