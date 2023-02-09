namespace Suinet.Rpc.Types
{
    public class SuiCertifiedTransactionEffects
    {
        public SuiTransactionEffects Effects { get; set; }

        public string TransactionEffectsDigest { get; set; }

        public object  FinalityInfo { get; set; }
    }
}
