namespace cbk.cloud.gcp.serviceProvider.KMS
{
    public interface IKmsService
    {
        byte[] Encrypt(string plaintext);
    }
}
