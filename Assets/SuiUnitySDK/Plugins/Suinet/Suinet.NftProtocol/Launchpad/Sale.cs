using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Launchpad
{
    public class Sale
    {
        public string Type { get; set; }

        public SaleFields Fields { get; set; }  
    }

    public class SaleFields
    {
        public UID Id { get; set; }

        [JsonProperty("tier_index")]
        public ulong TierIndex { get; set; }

        public bool Whitelisted { get; set; }

        public IEnumerable<string> Nfts { get; set; }

        public IEnumerable<string> Queue { get; set; }

        public object Market { get; set; }
    }
}
