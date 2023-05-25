using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class DisplayData
    {
        [JsonProperty("description")]
        public string Attributes { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("link")]
        public string ExplorerLink { get; set; }

        [JsonProperty("project_url")]
        public string ProjectUrl { get; set; }
    }

}
