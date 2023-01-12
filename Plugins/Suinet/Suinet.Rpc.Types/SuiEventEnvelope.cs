namespace Suinet.Rpc.Types
{
    public class SuiEventEnvelope
    {
        public SuiEvent Event { get; set; }

        public ulong Timestamp { get; set; }

        public string TxDigest { get; set; }
    }
}
