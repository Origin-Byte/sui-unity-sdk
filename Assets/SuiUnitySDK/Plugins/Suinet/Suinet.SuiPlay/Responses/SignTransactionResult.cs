using Newtonsoft.Json;

namespace Suinet.SuiPlay.Responses
{
    public class SignTransactionResult
    {
        [JsonProperty("signature")]
        public SignatureResult Signature { get; set; }
    }
    
    public class SignatureResult
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("txDigest")]
        public string TxDigest { get; set; }
    }
}