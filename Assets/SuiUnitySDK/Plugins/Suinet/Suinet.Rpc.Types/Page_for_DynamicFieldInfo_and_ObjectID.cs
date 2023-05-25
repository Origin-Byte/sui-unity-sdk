using Newtonsoft.Json;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class Page_for_DynamicFieldInfo_and_ObjectID
    {
        [JsonProperty("data")]
        public IEnumerable<DynamicFieldInfo> Data { get; set; }

        [JsonProperty("nextCursor")]
        public string NextCursor { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
