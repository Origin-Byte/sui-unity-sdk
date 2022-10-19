namespace Suinet.Rpc.Types
{
    public class SuiCertifiedTransactionEffects
    {
        public SuiAuthorityQuorumSignInfo AuthSignInfo { get; set; }

        public SuiTransactionEffects Effects { get; set; }

        public string TransactionEffectsDigest { get; set; }
    }
}
