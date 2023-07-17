namespace cbk.cloud.serviceProvider.CloudRun.EnviromentConfig
{
    public interface IDBEnvironmentConfig
    {
        string InstanceName { get; }
        string DatabaseName { get; }
        string UserName { get; }
        string Password { get; }
        string SeverCertificatePath { get; }
        string ClientCertificatePath { get; }
        string ClientCertificateKeyPath { get; }

    }

    // 注入抽換Mock使用
    public interface IDBEnvironmentConfigFactory
    {
        IDBEnvironmentConfig Create();
    }
}
