using Newtonsoft.Json;

namespace Suinet.SuiPlay.Requests
{
    public class SignTransactionRequest
    {
        [JsonProperty("txBytes")]
        public string TxBytes { get; set; }
    }
}
