namespace Suinet.Rpc.Types
{
    public class SuiCertifiedTransaction
    {
        public string TransactionDigest { get; set; }

        public SuiTransactionData Data { get; set; }

        public string TxSignature { get; set; }

        public SuiAuthorityQuorumSignInfo AuthSignInfo { get; set; }
    }
}
