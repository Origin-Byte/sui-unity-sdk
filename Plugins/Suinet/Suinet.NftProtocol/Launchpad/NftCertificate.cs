using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Launchpad
{
    public class NftCertificate
    {
        public UID Id { get; set; }

        [JsonProperty("launchpad_id")]
        public string LaunchpadId { get; set; }

        [JsonProperty("nft_id")]
        public string NftId { get; set; }
    }
}
