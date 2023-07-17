namespace cbk.cloud.serviceProvider.CloudRun.EnviromentConfig
{
    public interface IEncryptionEnvironmentConfig
    {
        string ProjectId { get; }
        string LocationId { get; }
        string KeyRingId { get; }
        string KeyId { get; }
        string KeyVersion { get; }
    }

    // 注入抽換Mock使用
    public interface IEncryptionEnvironmentConfigFactory
    {
        IEncryptionEnvironmentConfig Create();
    }
}
