using Google.Cloud.SecretManager.V1;

namespace cbk.cloud.serviceProvider.SecretManager
{
    public class GoogleSecretManager : ISecretManager
    {
        
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
