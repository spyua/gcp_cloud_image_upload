namespace cbk.cloudUploadImage.Infrastructure.Database.DBConnection.Model
{
    public class DBConnectionSetting : IDBConnectionModel
    {
        /// <summary>
        /// DB Instance Name
        /// </summary>
        public string InstanceName { get; set; } = string.Empty;

        /// <summary>
        /// DB Schema Name
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Login User Name
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Login User Password
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// SeverCertificatePath: The path to the certificate file used by the server.
        /// </summary>
        public string SeverCertificatePath { get; set; } = string.Empty;

        /// <summary>
        /// ClientCertificatePath: The path to the certificate file used by the client.
        /// </summary>
        public string ClientCertificatePath { get; set; } = string.Empty;

        /// <summary>
        /// ClientCertificateKeyPath: The path to the certificate key file used by the client.
        /// </summary>
        public string ClientCertificateKeyPath { get; set; } = string.Empty;

    }
}
