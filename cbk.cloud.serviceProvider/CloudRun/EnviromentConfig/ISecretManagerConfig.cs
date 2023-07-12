namespace cbk.cloud.serviceProvider.CloudRun.EnviromentConfig
{
    public interface ISecretManagerConfig
    {
        public string SecretId { get; }

        public string ProjectId { get; }

        public string VersionId { get; }
    }
}
