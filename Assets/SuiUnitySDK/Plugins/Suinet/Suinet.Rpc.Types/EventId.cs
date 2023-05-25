using Newtonsoft.Json;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class EventId
    {
        [JsonProperty("eventSeq")]
        public BigInteger EventSeq { get; set; }

        [JsonProperty("txDigest")]
        public string TxDigest { get; set; }
    }
}
