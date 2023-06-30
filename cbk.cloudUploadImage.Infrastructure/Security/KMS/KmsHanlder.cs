using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using cbk.cloud.gcp.serviceProvider.KMS;
using cbk.cloudUploadImage.Infrastructure.Config.KMSEncryption;

namespace cbk.cloudUploadImage.Infrastructure.Security.KMS
{
    public class KmsHanlder
    {
        private readonly IKmsService _kmsService;
        private readonly IEncryptionEnvironmentConfig _encryptionEnvironmentConfig;
        public KmsHanlder(IKmsService kmsService
                        , IEncryptionEnvironmentConfig encryptionEnvironmentConfig)
        {
            _kmsService = kmsService;
            _encryptionEnvironmentConfig = encryptionEnvironmentConfig;
        }

        public byte[] Encrypt(string plaintext) => _kmsService.Encrypt(plaintext);
    }
}
