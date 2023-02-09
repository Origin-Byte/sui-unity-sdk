using Newtonsoft.Json;
using Suinet.Rpc.Types.Converters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiExecuteTransactionResponseConverter))]
    public class SuiExecuteTransactionResponse
    {
        public SuiExecuteTransactionRequestType ExecuteTransactionRequestType { get; set; }

        public SuiCertifiedTransaction Certificate { get; set; }

        public SuiCertifiedTransactionEffects Effects { get; set; }

        [JsonProperty("confirmed_local_execution")]
        public bool ConfirmedLocalExecution { get; set; }
    }
}
