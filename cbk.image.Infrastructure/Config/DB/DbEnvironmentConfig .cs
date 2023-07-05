using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.Config.DB
{
    public class DBEnvironmentConfig : IDBEnvironmentConfig
    {
        public string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        // Wait 抽換
        //public string certFolder { get { return "Files\\CertificateFile"; } }
        //private string certFolder = "Files/CertificateFile";
        public string CertFolder { get { return $"Files{Path.DirectorySeparatorChar}CertificateFile"; } }
        public string InstanceName { get;  set; }
        public string DatabaseName { get;  set; }
        public string UserName { get;  set; }
        public string Password { get;  set; }
        public string SeverCertificatePath { get;  set; }
        public string ClientCertificatePath { get;  set; }
        public string ClientCertificateKeyPath { get;  set; }
        public DBEnvironmentConfig(bool useMock = false)
        {
            if (useMock)
            {
                // Set Mock Data
                // Wait抽換
                InstanceName = "35.229.242.171";
                //InstanceName = "172.21.240.3";
                DatabaseName = "postgres";
                UserName = "cbk_testing";
                Password = "cbktesting";
                SeverCertificatePath = Path.Combine(BaseDirectory, CertFolder, "server-ca.pem");
                ClientCertificatePath = Path.Combine(BaseDirectory, CertFolder, "client-cert.pem");
                ClientCertificateKeyPath = Path.Combine(BaseDirectory, CertFolder, "client-key.pem");
            }
            else
            {
                // Get environment variable or set to default
                InstanceName = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
                DatabaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
                UserName = Environment.GetEnvironmentVariable("DB_USER") ?? "";
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
                SeverCertificatePath = Path.Combine(BaseDirectory,CertFolder,Environment.GetEnvironmentVariable("DB_SERVER_CA") ?? "");
                ClientCertificatePath = Path.Combine(BaseDirectory,CertFolder,Environment.GetEnvironmentVariable("DB_CLIENT_CERT") ?? "");
                ClientCertificateKeyPath = Path.Combine(BaseDirectory,CertFolder,Environment.GetEnvironmentVariable("DB_CLIENT_KEY") ?? "");
            }
        }
    }
}
