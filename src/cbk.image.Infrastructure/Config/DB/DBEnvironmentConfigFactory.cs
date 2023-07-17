using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.Config.DB
{
    public class DBEnvironmentConfigFactory : IDBEnvironmentConfigFactory
    {
        public IDBEnvironmentConfig Create()
        {
            var config = new DBEnvironmentConfig();
            config.InstanceName = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
            config.DatabaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
            config.UserName = Environment.GetEnvironmentVariable("DB_USER") ?? "";
            config.Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            config.SeverCertificatePath = Environment.GetEnvironmentVariable("DB_SERVER_CA") ?? "";
            config.ClientCertificatePath = Environment.GetEnvironmentVariable("DB_CLIENT_CERT") ?? "";
            config.ClientCertificateKeyPath = Environment.GetEnvironmentVariable("DB_CLIENT_KEY") ?? "";
            //config.SeverCertificatePath = "/etc/secrets/sql-server-ca/upload-image-cloud-sql-server-ca";
            //config.ClientCertificatePath = "/etc/secrets/sql-client-cert/upload-image-cloud-sql-client-cert";
            //config.ClientCertificateKeyPath = "/etc/secrets/sql-client-key/upload-image-cloud-sql-client-key";
            //config.SeverCertificatePath = Path.Combine(config.BaseDirectory, config.CertFolder, Environment.GetEnvironmentVariable("DB_SERVER_CA") ?? "");
            //config.ClientCertificatePath = Path.Combine(config.BaseDirectory, config.CertFolder, Environment.GetEnvironmentVariable("DB_CLIENT_CERT") ?? "");
            //config.ClientCertificateKeyPath = Path.Combine(config.BaseDirectory, config.CertFolder, Environment.GetEnvironmentVariable("DB_CLIENT_KEY") ?? "");
            return config;
        }
        
    }
}
