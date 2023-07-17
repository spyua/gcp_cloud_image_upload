namespace cbk.cloud.serviceProvider.SecretManager
{
    public interface ISecretManagerService
    {
        string AccessSecretVersion();

        string AccessSecretVersion(string secretId, string projectId, string versionId);
    }
}
