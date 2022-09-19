namespace Suinet.Rpc.Types
{
    public class SuiObjectRef
    {
        public string ObjectId { get; set; }

        public ulong Version { get; set; }

        public string Digest { get; set; }
    }
}
