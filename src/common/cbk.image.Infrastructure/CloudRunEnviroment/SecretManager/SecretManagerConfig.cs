using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.CloudRunEnviroment.SecretManager
{
    public class SecretManagerConfig : ISecretManagerConfig
    {
        public string SecretId { get; private set; }

        public string ProjectId { get; private set; }

        public string VersionId { get; private set; }

        public SecretManagerConfig(bool useMock = false)
        {
            if (useMock)
            {
                SecretId = "upload-image-project-token-key";
                ProjectId = "1083093269039";
                VersionId = "1";
            }
            else
            {
                SecretId = Environment.GetEnvironmentVariable("SECRET_ID") ?? "";
                ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID") ?? "";
                VersionId = Environment.GetEnvironmentVariable("VERSION_ID") ?? "";
            }           
        }
    }
}
