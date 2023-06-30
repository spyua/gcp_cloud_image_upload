namespace cbk.cloud.serviceProvider.CloudRun.EnviromentConfig
{
    public interface IDbEnvironmentConfig
    {
        string InstanceName { get; }
        string DatabaseName { get; }
        string UserName { get; }
        string Password { get; }
        string SeverCertificatePath { get; }
        string ClientCertificatePath { get; }
        string ClientCertificateKeyPath { get; }

    }
}
