using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.Config.KMSEncryption
{
    public class MockEncryptionConfigFactory : IEncryptionEnvironmentConfigFactory
    {
        public IEncryptionEnvironmentConfig Create()
        {
            return new EncryptionEnvironmentConfig
            {
                ProjectId = "affable-cacao-389805",
                LocationId = "asia-east1",
                KeyRingId = "cathy-sample-project",
                KeyId = "cathy-sample-project-login-usage",
                KeyVersion = "1"
            };
        }
    }
}
