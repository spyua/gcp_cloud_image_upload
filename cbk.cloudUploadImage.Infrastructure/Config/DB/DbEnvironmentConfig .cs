namespace cbk.cloudUploadImage.Infrastructure.Config.DB
{
    public class DbEnvironmentConfig : IDbEnvironmentConfig
    {
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string certFolder = "Certificate";
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
                InstanceName = "mock_host";
                DatabaseName = "mock_db";
                UserName = "mock_user";
                Password = "mock_password";
                SeverCertificatePath = Path.Combine(baseDirectory, certFolder, "mock_ca");
                ClientCertificatePath = Path.Combine(baseDirectory, certFolder, "mock_cert");
                ClientCertificateKeyPath = Path.Combine(baseDirectory, certFolder, "mock_key");
            }
            else
            {
                // Get environment variable or set to default
                InstanceName = Environment.GetEnvironmentVariable("DB_HOST") ?? "35.229.242.171";
                DatabaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
                UserName = Environment.GetEnvironmentVariable("DB_USER") ?? "cbk_testing";
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "cbktesting";
                SeverCertificatePath = Path.Combine(
                    baseDirectory,
                    certFolder,
                    Environment.GetEnvironmentVariable("DB_SERVER_CA") ?? "server-ca.pem"
                );
                ClientCertificatePath = Path.Combine(
                    baseDirectory,
                    certFolder,
                    Environment.GetEnvironmentVariable("DB_CLIENT_CERT") ?? "client-cert.pem"
                );
                ClientCertificateKeyPath = Path.Combine(
                    baseDirectory,
                    certFolder,
                    Environment.GetEnvironmentVariable("DB_CLIENT_KEY") ?? "client-key.pem"
                );
            }
        }
    }
}
