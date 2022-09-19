using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class SuiTransactionResponse
    {
        public SuiCertifiedTransaction Certificate { get; set; }

        public object Effects { get; set; }

        [JsonProperty("parsed_data")]
        public object ParsedData { get; set; }

        [JsonProperty("timestamp_ms")]
        public ulong? TimestampMs { get; set; }
    }
}
