namespace Suinet.SuiPlay.Responses
{
    public class SignTransactionResult
    {
        public SignatureResult Signature { get; set; }
    }
    
    public class SignatureResult
    {
        public string Signature { get; set; }
        public string TxDigest { get; set; }
    }
}