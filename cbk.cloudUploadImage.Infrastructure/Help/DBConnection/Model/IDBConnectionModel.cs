namespace cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model
{
    public interface IDBConnectionModel
    {
        /// <summary>
        /// DB Instance Name
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// DB Schema Name
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Login User Name
        /// </summary>
        public string UserName { get; set; } 

        /// <summary>
        /// Login User Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// SeverCertificatePath: The path to the certificate file used by the server.
        /// </summary>
        public string SeverCertificatePath { get; set; }

        /// <summary>
        /// ClientCertificatePath: The path to the certificate file used by the client.
        /// </summary>
        public string ClientCertificatePath { get; set; }

        /// <summary>
        /// ClientCertificateKeyPath: The path to the certificate key file used by the client.
        /// </summary>
        public string ClientCertificateKeyPath { get; set; }
    }
}
