namespace cbk.cloudUploadImage.Infrastructure.Config.DB
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
