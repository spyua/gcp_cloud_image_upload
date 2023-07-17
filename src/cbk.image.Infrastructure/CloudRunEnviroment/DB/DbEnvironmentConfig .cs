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
        public string InstanceName { get;  set; }
        public string DatabaseName { get;  set; }
        public string UserName { get;  set; }
        public string Password { get;  set; }
        public string SeverCertificatePath { get;  set; }
        public string ClientCertificatePath { get;  set; }
        public string ClientCertificateKeyPath { get;  set; }
        
    }
}
