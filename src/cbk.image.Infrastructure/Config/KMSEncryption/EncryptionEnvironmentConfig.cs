using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.Config.KMSEncryption
{
    public class EncryptionEnvironmentConfig : IEncryptionEnvironmentConfig
    {
        public string ProjectId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public string KeyRingId { get; set; } = string.Empty;
        public string KeyId { get; set; } = string.Empty;
        public string KeyVersion { get; set; } = string.Empty;
    }
}
