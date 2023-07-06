using cbk.cloud.serviceProvider.Eventarc.EventModel;
using Newtonsoft.Json;

namespace cbk.cloud.serviceProvider.Eventarc.Model
{
    // 如果JSON名稱沒大小寫問題，可以不用JsonProperty
    public class StorageEvent : BaseEvent
    {

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("selfLink")]
        public string SelfLink { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("bucket")]
        public string Bucket { get; set; } = string.Empty;

        [JsonProperty("generation")]
        public string Generation { get; set; } = string.Empty;

        [JsonProperty("metageneration")]
        public string Metageneration { get; set; } = string.Empty;

        [JsonProperty("timeCreated")]
        public string TimeCreated { get; set; } = string.Empty;

        [JsonProperty("updated")]
        public string Updated { get; set; } = string.Empty;

        [JsonProperty("storageClass")]
        public string StorageClass { get; set; } = string.Empty;

        [JsonProperty("timeStorageClassUpdated")]
        public string TimeStorageClassUpdated { get; set; } = string.Empty;

        [JsonProperty("size")]
        public string Size { get; set; } = string.Empty;

        [JsonProperty("md5Hash")]
        public string Md5Hash { get; set; } = string.Empty;

        [JsonProperty("mediaLink")]
        public string MediaLink { get; set; } = string.Empty;

        [JsonProperty("crc32c")]
        public string Crc32c { get; set; } = string.Empty;

        [JsonProperty("etag")]
        public string Etag { get; set; } = string.Empty;
    }

}
