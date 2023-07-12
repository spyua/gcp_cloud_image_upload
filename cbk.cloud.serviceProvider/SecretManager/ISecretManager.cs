namespace cbk.cloud.serviceProvider.SecretManager
{
    public interface ISecretManager
    {
        string AccessSecretVersion(string secretId, string projectId, string versionId);
    }
}
