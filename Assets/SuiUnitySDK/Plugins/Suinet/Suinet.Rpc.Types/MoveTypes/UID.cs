using Newtonsoft.Json;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class UID
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
