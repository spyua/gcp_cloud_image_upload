using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using Google.Cloud.Kms.V1;
using Google.Protobuf;
using Google.Type;

namespace cbk.cloud.gcp.serviceProvider.KMS
{
    public class GoogleKmsService : IKmsService
    {
        private readonly KeyManagementServiceClient _client;
        private readonly CryptoKeyVersionName _keyVersionName;

        public GoogleKmsService(IEncryptionEnvironmentConfig environmentConfig)
        {
            _client = KeyManagementServiceClient.Create();
            _keyVersionName = new CryptoKeyVersionName(environmentConfig.ProjectId
                                                    , environmentConfig.LocationId
                                                    , environmentConfig.KeyRingId
                                                    , environmentConfig.KeyId
                                                    , environmentConfig.KeyVersion);

        }

        public GoogleKmsService(string projectId, string locationId, string keyRingId, string keyId, string keyVersion)
        {
            _client = KeyManagementServiceClient.Create();
            _keyVersionName = new CryptoKeyVersionName(projectId, locationId, keyRingId, keyId, keyVersion);

        }

        public byte[] Encrypt(string plaintext)
        {
            ByteString dataByteString = ByteString.CopyFromUtf8(plaintext);
            MacSignResponse result = _client.MacSign(_keyVersionName, dataByteString);
            byte[] signature = result.Mac.ToByteArray();
            return signature;
        }
    }
}
