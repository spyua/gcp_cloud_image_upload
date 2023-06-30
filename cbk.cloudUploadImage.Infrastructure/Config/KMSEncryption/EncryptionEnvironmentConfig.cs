using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.cloudUploadImage.Infrastructure.Config.KMSEncryption
{
    public class EncryptionEnvironmentConfig : IEncryptionEnvironmentConfig
    {
        public string ProjectId { get; private  set; }
        public string LocationId { get; private set; }
        public string KeyRingId { get; private set; }
        public string KeyId { get; private set; }
        public string KeyVersion { get; private set; }

        public EncryptionEnvironmentConfig(bool useMock = false)
        {
            if (useMock)
            {
                // Set Mock Data
                ProjectId = "affable-cacao-389805";
                LocationId = "asia-east1";
                KeyRingId = "cathy-sample-project";
                KeyId = "cathy-sample-project-login-usage";
                KeyVersion = "1";
            }
            else
            {
                // Get environment variable or set to default
                ProjectId = Environment.GetEnvironmentVariable("KMS_PROJECT_ID") ?? "";
                LocationId = Environment.GetEnvironmentVariable("KMS_LOCATION_ID") ?? "";
                KeyRingId = Environment.GetEnvironmentVariable("KMS_KEY_RING_ID") ?? "";
                KeyId = Environment.GetEnvironmentVariable("KMS_KEY_ID") ?? "";
                KeyVersion = Environment.GetEnvironmentVariable("KMS_KEY_VERSION") ?? "";
            }
        }   
    }
}
