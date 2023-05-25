using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types
{
    public class DynamicFieldInfo
    {
        [JsonProperty("name")]
        public DynamicFieldName Name { get; set; }

        [JsonProperty("bcsName")]
        public string BcsName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("objectType")]
        public MoveType ObjectType { get; set; }

        [JsonProperty("objectId")]
        public string ObjectId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("digest")]
        public string Digest { get; set; }
    }

    public class DynamicFieldName
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
