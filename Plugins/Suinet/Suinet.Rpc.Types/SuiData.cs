using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class SuiData
    {
        public string DataType { get; set; }

        /// <summary>
        /// Move type (e.g., "0x2::coin::Coin<0x2::sui::SUI>")
        /// </summary>
        public string Type { get; set; }

        [JsonProperty("has_public_transfer")]
        public bool HasPublicTransfer { get; set; }

        public Dictionary<string, object> Fields { get; set; }      
    }
}
