using Newtonsoft.Json;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class Coin
    {
        [JsonProperty("balance")]
        public BigInteger Balance { get; set; }

        [JsonProperty("coinObjectId")]
        public string CoinObjectId { get; set; }

        [JsonProperty("coinType")]
        public string CoinType { get; set; }

        [JsonProperty("digest")]
        public string Digest { get; set; }

        [JsonProperty("previousTransaction")]
        public string PreviousTransaction { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }
}
