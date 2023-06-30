using Google.Cloud.Kms.V1;
using Google.Protobuf;
using Google.Type;

namespace cbk.cloud.gcp.serviceProvider.KMS
{
    public class GoogleKmsService : IKmsService
    {
        private readonly KeyManagementServiceClient _client;
        private readonly CryptoKeyVersionName _keyVersionName;

        public GoogleKmsService(string projectId, string locationId, string keyRingId, string keyId, string keyVersion)
        {
            // projects/affable-cacao-389805/locations/asia-east1/keyRings/cathy-sample-project/cryptoKeys/cathy-sample-project-login-usage/cryptoKeyVersions/1
            // projects/affable-cacao-389805/locations/asia-east1/keyRings/cathy-sample-project/cryptoKeys/cathy-sample-project-login-usage/cryptoKeyVersions/1
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
