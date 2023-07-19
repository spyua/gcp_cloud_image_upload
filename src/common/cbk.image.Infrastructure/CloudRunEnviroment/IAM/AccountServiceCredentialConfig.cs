namespace cbk.image.Infrastructure.CloudRunEnviroment.IAM
{
    public class AccountServiceCredentialConfig
    {
        private string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string CertFolder = $"Files{Path.DirectorySeparatorChar}CertificateFile";
        public string CredentialFilePath { get; private set; }

        public AccountServiceCredentialConfig(bool useMock = false)
        {
            if (useMock)
            {
                // Set Mock Data
                CredentialFilePath = Path.Combine(BaseDirectory, CertFolder, "affable-cacao-389805-297d12d69696.json");
            }
            else
            {
                CredentialFilePath = Environment.GetEnvironmentVariable("ACCOUNT_SERVICE") ?? "";
            }
        }
    }
}
