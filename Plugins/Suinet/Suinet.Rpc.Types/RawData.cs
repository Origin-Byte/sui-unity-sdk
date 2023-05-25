using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public abstract class RawData
    {
        [JsonProperty("dataType")]
        public string DataType { get; set; }
    }

    public class MoveObjectRawData : RawData
    {
        [JsonProperty("bcsBytes")]
        public string BcsBytes { get; set; } 

        [JsonProperty("hasPublicTransfer")]
        public bool HasPublicTransfer { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public BigInteger Version { get; set; }
    }

    public class PackageRawData : RawData
    {
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        [JsonProperty("linkageTable")]
        public Dictionary<string, UpgradeInfo> LinkageTable { get; set; }

        [JsonProperty("moduleMap")]
        public Dictionary<string, string> ModuleMap { get; set; }

        [JsonProperty("typeOriginTable")]
        public List<TypeOrigin> TypeOriginTable { get; set; }

        [JsonProperty("version")]
        public BigInteger Version { get; set; }
    }
}
