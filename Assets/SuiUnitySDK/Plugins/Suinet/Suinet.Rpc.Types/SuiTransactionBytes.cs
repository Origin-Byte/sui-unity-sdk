namespace Suinet.Rpc.Types
{
    public class SuiTransactionBytes
    {
        public SuiObjectRef Gas { get; set; }

        public object[] InputObjects { get; set; }

        public string TxBytes { get; set; }
    }
}
