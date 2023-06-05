using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class Page_for_Coin_and_ObjectID
    {
        [JsonProperty("data")]
        public Coin[] Data { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("nextCursor")]
        public string NextCursor { get; set; }
    }
}
