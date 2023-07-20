namespace Suinet.SuiPlay.Requests
{
    public class SuiTransactionRequest
    {
        public string TxBytes { get; set; }
        public long GasBudget { get; set; }
        public string RequestType { get; set; }
    }
}
