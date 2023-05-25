using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    public class TransactionBlockData
    {
        [JsonProperty("gasData")]
        public GasData GasData { get; set; }

        [JsonProperty("messageVersion")]
        public string MessageVersion { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonConverter(typeof(TransactionBlockKindJsonConverter))]
        [JsonProperty("transaction")]
        public TransactionBlockKind Transaction { get; set; }

        public TransactionBlockData()
        {
            MessageVersion = "v1"; 
        }
    }
}
