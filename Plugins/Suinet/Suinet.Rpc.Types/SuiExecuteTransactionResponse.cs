using Newtonsoft.Json;
using Suinet.Rpc.Types.Converters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiExecuteTransactionResponseConverter))]
    public class SuiExecuteTransactionResponse
    {
        public SuiExecuteTransactionRequestType ExecuteTransactionRequestType { get; set; }

        /// <summary>
        /// SuiExecuteTransactionRequestType.ImmediateReturn
        /// </summary>
        public SuiImmediateReturn ImmediateReturn { get; set; }

        /// <summary>
        /// SuiExecuteTransactionRequestType.WaitForTxCert
        /// </summary>
        public SuiTxCert TxCert { get; set; }

        /// <summary>
        /// SuiExecuteTransactionRequestType.WaitForEffectsCert
        /// </summary>
        public SuiEffectsCert EffectsCert { get; set; }

        public class SuiImmediateReturn
        {
            [JsonProperty("tx_digest")]
            public string TxDigest { get; set; }
        }

        public class SuiTxCert 
        {
            public SuiCertifiedTransaction Certificate { get; set; }
        }

        public class SuiEffectsCert
        {
            public SuiCertifiedTransaction Certificate { get; set; }

            public SuiCertifiedTransactionEffects Effects { get; set; }
        }
    }
}
