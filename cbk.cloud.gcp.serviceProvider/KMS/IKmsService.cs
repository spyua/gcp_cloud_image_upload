namespace cbk.cloud.serviceProvider.KMS
{
    public interface IKmsService
    {
        byte[] Encrypt(string plaintext);
    }
}
