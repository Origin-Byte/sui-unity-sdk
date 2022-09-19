namespace Suinet.Rpc.Types
{
    public class SuiObjectInfo
    {
        public string ObjectId { get; set; }

        public ulong Version { get; set; }

        public string Digest { get; set; }

        public string Type { get; set; }

        public object Owner { get; set; }

        public string PreviousTransaction { get; set; }
    }
}
