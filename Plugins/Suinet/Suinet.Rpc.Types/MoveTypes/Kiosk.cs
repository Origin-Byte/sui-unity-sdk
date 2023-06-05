using Newtonsoft.Json;
using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class Kiosk
    {
        public UID Id { get; set; }
        
        public object Profits { get; set; }

	    public string Owner { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }

        [JsonProperty("allow_extensions")]
        public bool AllowExtensions { get; set; }
    }
}
