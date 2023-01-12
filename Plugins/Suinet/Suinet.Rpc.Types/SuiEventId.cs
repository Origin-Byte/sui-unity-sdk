using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class SuiEventId
    {
        [JsonProperty("eventSeq")]
        public long EventSeq { get; set; }

        [JsonProperty("txSeq")]
        public long TxSeq { get; set; }
    }
}
