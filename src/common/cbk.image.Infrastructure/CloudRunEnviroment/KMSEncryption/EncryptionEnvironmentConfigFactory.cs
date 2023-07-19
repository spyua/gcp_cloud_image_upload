using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.CloudRunEnviroment.KMSEncryption
{
    public class EncryptionEnvironmentConfigFactory : IEncryptionEnvironmentConfigFactory
    {
        public IEncryptionEnvironmentConfig Create()
        {
            return new EncryptionEnvironmentConfig
            {
                ProjectId = Environment.GetEnvironmentVariable("KMS_PROJECT_ID") ?? "",
                LocationId = Environment.GetEnvironmentVariable("KMS_LOCATION_ID") ?? "",
                KeyRingId = Environment.GetEnvironmentVariable("KMS_KEY_RING_ID") ?? "",
                KeyId = Environment.GetEnvironmentVariable("KMS_KEY_ID") ?? "",
                KeyVersion = Environment.GetEnvironmentVariable("KMS_KEY_VERSION") ?? ""
            };
        }    
    }
}
