using Newtonsoft.Json;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class DisplayFieldsResponse
    {
        [JsonProperty("data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonProperty("error")]
        public ObjectResponseError Error { get; set; }
    }
}
