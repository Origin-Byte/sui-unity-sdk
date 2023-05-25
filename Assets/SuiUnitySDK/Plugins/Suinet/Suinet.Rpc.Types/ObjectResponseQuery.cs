using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class ObjectResponseQuery
    {
        [JsonProperty("filter")]
        public ObjectDataFilter Filter { get; set; } = null;

        [JsonProperty("options")]
        public ObjectDataOptions Options { get; set; } = null;
    }
}
