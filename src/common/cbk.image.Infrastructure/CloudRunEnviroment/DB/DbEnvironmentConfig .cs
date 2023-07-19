using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.CloudRunEnviroment.DB
{
    public class DBEnvironmentConfig : IDBEnvironmentConfig
    {
        public string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        // Wait 抽換
        //public string certFolder { get { return "Files\\CertificateFile"; } }
        //private string certFolder = "Files/CertificateFile";
        public string CertFolder { get { return $"Files{Path.DirectorySeparatorChar}CertificateFile"; } }
        public string InstanceName { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SeverCertificatePath { get; set; } = string.Empty;
        public string ClientCertificatePath { get; set; } = string.Empty;
        public string ClientCertificateKeyPath { get; set; } = string.Empty;
            
    }
}
