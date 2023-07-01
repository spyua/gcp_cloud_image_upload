using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.Config.DB
{
    public class DbEnvironmentConfig : IDbEnvironmentConfig
    {
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //private string certFolder = "Files\\CertificateFile";
        private string certFolder = "Files/CertificateFile";

        public string InstanceName { get; private set; }
        public string DatabaseName { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string SeverCertificatePath { get; private set; }
        public string ClientCertificatePath { get; private set; }
        public string ClientCertificateKeyPath { get; private set; }
        public DbEnvironmentConfig(bool useMock = false)
        {
            if (useMock)
            {
                // Set Mock Data
                //InstanceName = "35.229.242.171";
                InstanceName = "172.21.240.3";
                DatabaseName = "postgres";
                UserName = "cbk_testing";
                Password = "cbktesting";
                SeverCertificatePath = Path.Combine(baseDirectory, certFolder, "server-ca.pem");
                ClientCertificatePath = Path.Combine(baseDirectory, certFolder, "client-cert.pem");
                ClientCertificateKeyPath = Path.Combine(baseDirectory, certFolder, "client-key.pem");
            }
            else
            {
                // Get environment variable or set to default
                InstanceName = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
                DatabaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
                UserName = Environment.GetEnvironmentVariable("DB_USER") ?? "";
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
                SeverCertificatePath = Path.Combine(baseDirectory,certFolder,Environment.GetEnvironmentVariable("DB_SERVER_CA") ?? "");
                ClientCertificatePath = Path.Combine(baseDirectory,certFolder,Environment.GetEnvironmentVariable("DB_CLIENT_CERT") ?? "");
                ClientCertificateKeyPath = Path.Combine(baseDirectory,certFolder,Environment.GetEnvironmentVariable("DB_CLIENT_KEY") ?? "");
            }
        }
    }
}
