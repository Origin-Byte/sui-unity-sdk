using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Nft
{
    // Base Nft type
    [MoveType("0x[a-f0-9]{40}::nft::Nft<.*>")]
    public class Nft
    {
        public UID Id { get; set; }

        public BagData Bag { get; set; }

        [JsonProperty("logical_owner")]
        public string LogicalOwner { get; set; }
    }
}
