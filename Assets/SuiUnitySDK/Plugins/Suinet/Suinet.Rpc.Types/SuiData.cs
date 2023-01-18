using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class SuiData
    {
        public string DataType { get; set; }

        public MoveType Type { get; set; }

        [JsonProperty("has_public_transfer")]
        public bool HasPublicTransfer { get; set; }

        public Dictionary<string, object> Fields { get; set; }      
    }
}
