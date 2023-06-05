using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class TransactionBlockResponseQuery
    {
        [JsonProperty("filter")]
        public TransactionFilter Filter { get; set; }

        [JsonProperty("options")]
        public TransactionBlockResponseOptions Options { get; set; }
    }
}
