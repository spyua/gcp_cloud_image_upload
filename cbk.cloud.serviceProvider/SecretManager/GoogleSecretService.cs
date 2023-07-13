using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;
using Google.Cloud.SecretManager.V1;

namespace cbk.cloud.serviceProvider.SecretManager
{

    public class GoogleSecretService : ISecretManagerService
    {
        public ISecretManagerConfig _secretManagerConfig { get; private set; }

        public GoogleSecretService(ISecretManagerConfig secretManagerConfig)
        {
            _secretManagerConfig = secretManagerConfig;
        }

        public string AccessSecretVersion()
        {
              return AccessSecretVersion(_secretManagerConfig.SecretId, _secretManagerConfig.ProjectId, _secretManagerConfig.VersionId);
        }

        public string AccessSecretVersion(string secretId, string projectId, string versionId)
        {
            // Create the client.
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            // Build the name of the secret version.
            SecretVersionName secretVersionName = new SecretVersionName(projectId, secretId, versionId);

            // Access the secret version.
            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            // Get the secret payload and convert it to a string.
            string secretData = result.Payload.Data.ToStringUtf8();

            // return the secret data.
            return secretData;
        }

    }
}
