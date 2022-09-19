namespace Suinet.Rpc.Types
{
    public class SuiObjectRead
    {
        public SuiObjectReadStatus Status { get; set; }

        public object Details { get; set; }
    }

    public enum SuiObjectReadStatus
    {
        Exists = 0,
        NotExists,
        Deleted
    }
}
