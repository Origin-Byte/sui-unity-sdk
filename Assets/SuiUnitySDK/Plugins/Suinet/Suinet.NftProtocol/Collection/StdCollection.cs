using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Collection
{
    [MoveType("0x[a-f0-9]{40}::collection::Collection<.*, 0x[a-f0-9]{40}::std_collection::StdMeta>")]
    public class StdCollection
    {
        public UID Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Symbol { get; set; }

        public string Receiver { get; set; }

        public IEnumerable<string> Tags { get; set; }

        [JsonProperty("is_mutable")]
        public bool IsMutable { get; set; }

        [JsonProperty("royalty_fee_bps")]
        public ulong RoyaltyFeeBps { get; set; }

        public IEnumerable<Creator> Creators { get; set; }

        [JsonProperty("mint_authority")]
        public string MintAuthority { get; set; }

        public object Metadata { get; set; }
    }

    public class Creator
    {
        [JsonProperty("creator_address")]
        public string CreatorAddress { get; set; }

        public bool Verified { get; set; }

        [JsonProperty("share_of_royalty")]
        public byte ShareOfRoyalty { get; set; }
    }
}
