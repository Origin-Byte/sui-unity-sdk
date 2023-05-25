using Newtonsoft.Json;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class UpgradeInfo
    {
        [JsonProperty("upgraded_id")]
        public ObjectId UpgradedId { get; set; }

        [JsonProperty("upgraded_version")]
        public BigInteger UpgradedVersion { get; set; }
    }
}
