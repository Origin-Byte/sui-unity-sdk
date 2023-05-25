using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Nft
{
    public class Nft
    {
        [JsonProperty("id")]
        public UID Id { get; set; }
    }
}
