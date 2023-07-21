using Newtonsoft.Json;

namespace Suinet.SuiPlay.Requests
{
    public class SuiTransactionRequest
    {
        [JsonProperty("txBytes")]
        public string TxBytes { get; set; }
        [JsonProperty("gasBudget")]
        public long GasBudget { get; set; }
        [JsonProperty("requestType")]
        public string RequestType { get; set; }
    }
}
