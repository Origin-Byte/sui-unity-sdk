using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Suinet.SuiPlay.Responses
{
    public class SuiTransactionResponseData
    {
        [JsonProperty("suiTransactionResponse")]
        public Dictionary<string, object> SuiTransactionResponse { get; set; }
    }
}
