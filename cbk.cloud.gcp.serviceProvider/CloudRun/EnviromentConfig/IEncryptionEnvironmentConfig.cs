namespace cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig
{
    public interface IEncryptionEnvironmentConfig
    {
        string ProjectId { get; }
        string LocationId { get; }
        string KeyRingId { get; }
        string KeyId { get; }
        string KeyVersion { get; }
    }
}
