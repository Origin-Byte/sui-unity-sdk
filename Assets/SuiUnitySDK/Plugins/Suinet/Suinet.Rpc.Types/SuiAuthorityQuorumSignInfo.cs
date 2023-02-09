using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class SuiAuthorityQuorumSignInfo
    {
        public int Epoch { get; set; }

        public string Signature { get; set; }

        [JsonProperty("signers_map")]
        public object SignersMap { get; set; }
    }
}
