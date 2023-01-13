using Newtonsoft.Json;

namespace Suinet.Rpc.Types.MoveTypes
{
    [MoveType("0x[a-f0-9]{40}::capy::Capy")]
    public class CapyNft
    {
        public UID Id { get; set; }

        public int Gen { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }

        public string Link { get; set; }
        public string Url { get; set; }

    }
}
